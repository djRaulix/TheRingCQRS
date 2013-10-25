namespace TheRing.CQRS.Eventing
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    #endregion

    public class DenormalizerMapper
    {
        #region Fields

        private readonly List<DenormalizerMapping> mappings = new List<DenormalizerMapping>();

        #endregion

        #region Public Properties

        public IEnumerable<DenormalizerMapping> Mappings
        {
            get
            {
                return this.mappings;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void AddMappings(Assembly assembly)
        {
            var denormalizerType = typeof(IDenormalizeEvent<>);

            var denormalizers = from type in assembly.GetExportedTypes()
                where !type.IsAbstract
                from @interface in
                    type.GetInterfaces()
                where @interface.IsGenericType && @interface.GetGenericTypeDefinition() == denormalizerType
                let eventType = @interface.GetGenericArguments().Single()
                group eventType by type
                into grp
                where grp.Any()
                select new DenormalizerMapping(grp, grp.Key);
            this.mappings.AddRange(denormalizers);
        }

        #endregion

        public class DenormalizerMapping
        {
            #region Constructors and Destructors

            public DenormalizerMapping(IEnumerable<Type> eventTypes, Type denormalizerType)
            {
                this.DenormalizerType = denormalizerType;
                this.EventTypes = eventTypes;
            }

            #endregion

            #region Public Properties

            public Type DenormalizerType { get; private set; }

            public IEnumerable<Type> EventTypes { get; private set; }

            #endregion
        }
    }
}