

using NServiceBus;
using NServiceBus.ObjectBuilder;

namespace NES.NServiceBus
{
    public class NeedInitialization : INeedInitialization
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.RegisterComponents(RegisterBusComponents);
        }

        private static void RegisterBusComponents(IConfigureComponents configureComponents)
        {
            if (!configureComponents.HasComponent<UnitOfWorkManager>())
            {
                configureComponents.ConfigureComponent<UnitOfWorkManager>(DependencyLifecycle.SingleInstance);
            }

            if (!configureComponents.HasComponent<ConfigurationRunner>())
            {
                configureComponents.ConfigureComponent<ConfigurationRunner>(DependencyLifecycle.InstancePerCall);
            }
        }
    }
}