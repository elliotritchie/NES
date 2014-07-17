// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandContextProvider.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The command context provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using global::NServiceBus;

    /// <summary>
    ///     The command context provider.
    /// </summary>
    public class CommandContextProvider : ICommandContextProvider
    {
        #region Static Fields

        private static readonly ILogger Logger = LoggerFactory.Create(typeof(CommandContextProvider));

        private static readonly Dictionary<Type, Func<object, Guid>> _cache = new Dictionary<Type, Func<object, Guid>>();

        private static readonly object _cacheLock = new object();

        #endregion

        #region Fields

        private readonly IBus _bus;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandContextProvider"/> class.
        /// </summary>
        /// <param name="bus">
        /// The bus.
        /// </param>
        public CommandContextProvider(IBus bus)
        {
            this._bus = bus;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The get.
        /// </summary>
        /// <returns>
        ///     The <see cref="CommandContext" />.
        /// </returns>
        public CommandContext Get()
        {
            var command = ExtensionMethods.CurrentMessageBeingHandled;
            var commandType = command.GetType();

            lock (_cacheLock)
            {
                Func<object, Guid> property;

                if (!_cache.TryGetValue(commandType, out property))
                {
                    var propertyInfo = commandType.GetProperty("Id");

                    if (propertyInfo != null)
                    {
                        Logger.Debug("Message Id property found for use as CommitId");

                        var commandParameter = Expression.Parameter(typeof(object), "command");
                        var propertyCall = Expression.Property(Expression.Convert(commandParameter, commandType), propertyInfo);

                        property = Expression.Lambda<Func<object, Guid>>(propertyCall, commandParameter).Compile();
                    }
                    else
                    {
                        Logger.Debug("Message Id property not found a CommitId will be automatically generated");

                        property = c => GuidComb.NewGuidComb();
                    }

                    _cache[commandType] = property;
                }

                return new CommandContext
                           {
                               Id = property(command), 
                               Headers =
                                   this._bus.CurrentMessageContext.Headers.Where(
                                       h =>
                                       !h.Key.Equals("CorrId", StringComparison.InvariantCultureIgnoreCase)
                                       && !h.Key.Equals("WinIdName", StringComparison.InvariantCultureIgnoreCase)
                                       && !h.Key.StartsWith("NServiceBus", StringComparison.InvariantCultureIgnoreCase))
                                   .ToDictionary(h => h.Key, h => (object)h.Value)
                           };
            }
        }

        #endregion
    }
}