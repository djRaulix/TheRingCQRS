namespace TheRing.CQRS.Commanding
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    #endregion

    public class CommandMapper
    {
        #region Fields

        private readonly Dictionary<Type, Type> mappings = new Dictionary<Type, Type>();

        #endregion

        #region Public Properties

        public IEnumerable<CommandMapping> Mappings
        {
            get
            {
                return from mapping in this.mappings
                       let commandHandler = typeof(CommandHandler<,>).MakeGenericType(mapping.Value, mapping.Key)
                    let handlesCommand = typeof(IHandlesCommand<>).MakeGenericType(mapping.Key)
                    select new CommandMapping(mapping.Key, commandHandler, handlesCommand);
            }
        }

        #endregion

        #region Public Methods and Operators

        public void AddMappings(Assembly assembly)
        {
            var aggregates = assembly.GetExportedTypes().Where(t => !t.IsAbstract && typeof(IAggregateRoot).IsAssignableFrom(t));
            foreach (var agg in aggregates)
            {
                var commandMethods = agg.GetMethods().Where(m => m.Name == "Run");
                foreach (var method in commandMethods)
                {
                    var args = method.GetParameters();
                    if (args.Count() != 1)
                    {
                        continue;
                    }

                    var arg = args.First().ParameterType;
                    if (arg.IsAbstract || !typeof(ICommand).IsAssignableFrom(arg))
                    {
                        continue;
                    }

                    try
                    {
                        this.mappings.Add(arg, agg);
                    }
                    catch
                    {
                        throw new Exception(
                            string.Format(
                                "Try to map {0} command to {1} aggregate but already mapped to {2}",
                                arg.Name,
                                agg.Name,
                                this.mappings[arg].Name));
                    }
                }
            }
        }

        #endregion

        public class CommandMapping
        {
            #region Constructors and Destructors

            public CommandMapping(Type commandType, Type commandHandlerType, Type handlesCommandType)
            {
                this.CommandHandlerType = commandHandlerType;
                this.CommandType = commandType;
                this.HandlesCommandType = handlesCommandType;
            }

            #endregion

            #region Public Properties

            public Type CommandHandlerType { get; private set; }

            public Type CommandType { get; private set; }

            public Type HandlesCommandType { get; private set; }

            #endregion
        }
    }
}