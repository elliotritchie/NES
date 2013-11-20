﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NServiceBus;

namespace NES.NServiceBus
{
    public class CommandContextProvider : ICommandContextProvider
    {
        private static readonly ILogger Logger = LoggingFactory.BuildLogger(typeof(CommandContextProvider));
        private static readonly Dictionary<Type, Func<object, Guid>> _cache = new Dictionary<Type, Func<object, Guid>>();
        private static readonly object _cacheLock = new object();
        private readonly IBus _bus;

        public CommandContextProvider(IBus bus)
        {
            _bus = bus;
        }

        public CommandContext Get()
        {
            Logger.Debug("Retrive the CurrentMessageBeingHandled");

            var command = ExtensionMethods.CurrentMessageBeingHandled;
            var commandType = command.GetType();

            lock (_cacheLock)
            {
                Func<object, Guid> property;

                if (!_cache.TryGetValue(commandType, out property))
                {
                    Logger.Debug("{0} is not on cache", commandType.FullName);
                    var propertyInfo = commandType.GetProperty("Id");

                    if (propertyInfo != null)
                    {
                        Logger.Debug("Command has property Id");
                        var commandParameter = Expression.Parameter(typeof(object), "command");
                        var propertyCall = Expression.Property(Expression.Convert(commandParameter, commandType), propertyInfo);

                        property = Expression.Lambda<Func<object, Guid>>(propertyCall, commandParameter).Compile();
                    }
                    else
                    {
                        Logger.Debug("Command has no property Id so the Id can't be automatically assigned");
                        property = c => GuidComb.NewGuidComb();
                    }

                    Logger.Debug("Add property Id to the cache of the command {0}", commandType.FullName);
                    _cache[commandType] = property;
                }

                return new CommandContext
                {
                    Id = property(command),
                    Headers = _bus.CurrentMessageContext.Headers
                        .Where(h =>
                            !h.Key.Equals("CorrId", StringComparison.InvariantCultureIgnoreCase) &&
                            !h.Key.Equals("WinIdName", StringComparison.InvariantCultureIgnoreCase) &&
                            !h.Key.StartsWith("NServiceBus", StringComparison.InvariantCultureIgnoreCase))
                        .ToDictionary(h => h.Key, h => (object)h.Value)
                };
            }
        }
    }
}