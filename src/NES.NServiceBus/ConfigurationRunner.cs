// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationRunner.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The configuration runner.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NServiceBus
{
    using global::NServiceBus;

    using global::NServiceBus.Config;

    using global::NServiceBus.Logging;

    /// <summary>
    ///     The configuration runner.
    /// </summary>
    public class ConfigurationRunner : IWantToRunWhenConfigurationIsComplete
    {
        #region Public Methods and Operators

        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public void Run(Configure config)
        {
            LoggerFactory.Create = t =>
                {
                    var logger = LogManager.GetLogger(t);
                    return new Logger(logger.DebugFormat, logger.InfoFormat, logger.WarnFormat, logger.ErrorFormat, logger.FatalFormat);
                };
        }

        #endregion
    }
}