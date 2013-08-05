namespace TheRing.CQRS.MassTransit
{
    using System;
    using System.Collections.Generic;

    using Magnum.Reflection;

    using global::MassTransit;

    using global::MassTransit.Saga;

    public class BusFactory : IBusFactory
    {
        // to reference explicitly rabbitmq
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

        public IServiceBus Set(string queue, IEnumerable<KeyValuePair<Type, Func<object>>> consumers = null, IEnumerable<Type> sagas = null)
        {
            var bus = ServiceBusFactory.New(
                sbc =>
                    {
                        sbc.UseRabbitMq();

                        sbc.ReceiveFrom("rabbitmq://" + queue);

                        sbc.Subscribe(
                            c =>
                                {
                                    if (consumers != null)
                                    {
                                        foreach (var consumer in consumers)
                                        {
                                            var getter = consumer.Value;
                                            c.Consumer(consumer.Key, t => getter).Permanent();
                                        }
                                    }

                                    if (sagas == null)
                                    {
                                        return;
                                    }

                                    foreach (var saga in sagas)
                                    {
                                        var repositoryType = typeof(ISagaRepository<>).MakeGenericType(saga);
                                        c.FastInvoke(new[] { saga }, "Saga", this.container(repositoryType));
                                    }
                                });
                    });

            this.buses[queue] = bus;

            return bus;
        }

        #endregion
    }
}