namespace TheRing.CQRS.MassTransit
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using global::MassTransit;

    using global::MassTransit.BusConfigurators;

    using global::MassTransit.Saga;

    using global::MassTransit.SubscriptionConfigurators;

    #endregion

    public class BusFactory : IBusFactory
    {
        #region Fields

        private readonly IDictionary<string, IServiceBus> buses;

        private readonly Func<Type, object> container;

        #endregion

        #region Constructors and Destructors

        public BusFactory(Func<Type, object> container)
        {
            this.buses = new Dictionary<string, IServiceBus>();
            this.container = container;
        }

        #endregion

        #region Public Methods and Operators

        public IServiceBus Get(string queue)
        {
            return this.buses[queue];
        }

        public IServiceBus Set(
            string queue, 
            IEnumerable<KeyValuePair<Type, Func<object>>> consumers = null, 
            IEnumerable<Type> sagas = null, 
            Action<ServiceBusConfigurator> moreConfig = null)
        {
            var bus = ServiceBusFactory.New(
                sbc =>
                {
                    if (moreConfig != null)
                    {
                        moreConfig(sbc);
                    }

                    sbc.ReceiveFrom(queue);

                    sbc.Subscribe(
                        c =>
                        {
                            if (consumers != null)
                            {
                                foreach (var consumer in consumers)
                                {
                                    var getter = consumer.Value;
                                    c.Consumer(consumer.Key, t => getter()).Permanent();
                                }
                            }

                            if (sagas == null)
                            {
                                return;
                            }

                            foreach (var saga in sagas)
                            {
                                var repositoryType = typeof(ISagaRepository<>).MakeGenericType(saga);
                                var method =
                                    typeof(BusFactory).GetMethod(
                                        "SetSaga", 
                                        BindingFlags.NonPublic | BindingFlags.Instance)
                                        .MakeGenericMethod(saga);
                                method.Invoke(this, new object[] { c });
                            }
                        });
                });

            this.buses[queue] = bus;

            return bus;
        }

        #endregion

        #region Methods

        private void SetSaga<TSaga>(SubscriptionBusServiceConfigurator configurator) where TSaga : class, ISaga
        {
            configurator.Saga((ISagaRepository<TSaga>)this.container(typeof(ISagaRepository<TSaga>))).Permanent();
        }

        #endregion
    }
}