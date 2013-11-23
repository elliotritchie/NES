using NServiceBus.Config;
using NServiceBus.Logging;

namespace NES.NServiceBus
{
    public class ConfigurationRunner : IWantToRunWhenConfigurationIsComplete
    {
        public void Run()
        {
            LoggerFactory.Create = t => 
            {
                var logger = LogManager.GetLogger(t);
                return new Logger(logger.DebugFormat, logger.InfoFormat, logger.WarnFormat, logger.ErrorFormat, logger.FatalFormat);
            };
        }
    }
}