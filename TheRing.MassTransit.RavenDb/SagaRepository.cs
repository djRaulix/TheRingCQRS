namespace TheRing.CQRS.MassTransit.RavenDb
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::MassTransit;
    using global::MassTransit.Exceptions;
    using global::MassTransit.Pipeline;
    using global::MassTransit.Saga;

    using Raven.Abstractions.Exceptions;
    using Raven.Client;
    using Raven.Client.Linq;

    #endregion

    public class SagaRepository<TSaga> : ISagaRepository<TSaga>
        where TSaga : class, ISaga
    {
        #region Fields

        private readonly IDocumentStore documentStore;

        #endregion

        #region Constructors and Destructors

        public SagaRepository(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<Guid> Find(ISagaFilter<TSaga> filter)
        {
            return this.Where(filter, x => x.CorrelationId);
        }

        public IEnumerable<Action<IConsumeContext<TMessage>>> GetSaga<TMessage>(
            IConsumeContext<TMessage> context, 
            Guid sagaId, 
            InstanceHandlerSelector<TSaga, TMessage> selector, 
            ISagaPolicy<TSaga, TMessage> policy) where TMessage : class
        {
            using (var session = this.documentStore.OpenSession())
            {
                session.Advanced.UseOptimisticConcurrency = true;

                var instance = session.Load<TSaga>(sagaId);

                if (instance == null)
                {
                    if (policy.CanCreateInstance(context))
                    {
                        yield return x =>
                        {
                            try
                            {
                                instance = policy.CreateInstance(x, sagaId);

                                foreach (var callback in selector(instance, x))
                                {
                                    callback(x);
                                }

                                if (policy.CanRemoveInstance(instance))
                                {
                                    return;
                                }

                                session.Store(instance);
                                session.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                var sex = new SagaException(
                                    "Create Saga Instance Exception", 
                                    typeof(TSaga), 
                                    typeof(TMessage), 
                                    sagaId, 
                                    ex);

                                throw sex;
                            }
                        };
                    }
                }
                else
                {
                    if (policy.CanUseExistingInstance(context))
                    {
                        yield return x =>
                        {
                            try
                            {
                                const int NbtriesMax = 5;

                                foreach (var callback in selector(instance, x))
                                {
                                    callback(x);
                                }

                                if (policy.CanRemoveInstance(instance))
                                {
                                    session.Delete(instance);
                                }

                                try
                                {
                                    session.SaveChanges();
                                }
                                catch (ConcurrencyException)
                                {
                                    if (x.RetryCount <= NbtriesMax)
                                    {
                                        x.RetryLater();
                                    }
                                    else
                                    {
                                        throw;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                var sex = new SagaException(
                                    "Existing Saga Instance Exception", 
                                    typeof(TSaga), 
                                    typeof(TMessage), 
                                    sagaId, 
                                    ex);

                                throw sex;
                            }
                        };
                    }
                }
            }
        }

        public IEnumerable<TResult> Select<TResult>(Func<TSaga, TResult> transformer)
        {
            using (var session = this.documentStore.OpenSession())
            {
                var conventions = this.documentStore.Conventions;
                return session.Advanced.LoadStartingWith<TSaga>(conventions.FindTypeTagName(typeof(TSaga)) + conventions.IdentityPartsSeparator).Select(transformer);
            }
        }

        public IEnumerable<TSaga> Where(ISagaFilter<TSaga> filter)
        {
            using (var session = this.documentStore.OpenSession())
            {
                RavenQueryStatistics statistics;

                session.Query<TSaga>()
                    .Where(filter.FilterExpression).Statistics(out statistics).Take(0).ToList();

                var enumerator = session.Advanced.Stream(
                    session.Query<TSaga>(statistics.IndexName)
                        .Where(filter.FilterExpression)
                        .Customize(c => c.WaitForNonStaleResultsAsOfLastWrite()));
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current.Document;
                }
            }
        }

        public IEnumerable<TResult> Where<TResult>(ISagaFilter<TSaga> filter, Func<TSaga, TResult> transformer)
        {
            return this.Where(filter).Select(transformer);
        }

        #endregion
    }
}