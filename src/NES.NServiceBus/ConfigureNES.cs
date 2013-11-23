using NServiceBus;
using NServiceBus.MessageInterfaces;
using NServiceBus.Serialization;

namespace NES.NServiceBus
{
    public static class ConfigureNES
    {
        public static Configure NES(this Configure config)
        {
            Global.TypesToScan = Configure.TypesToScan;
            
            config.Configurer.ConfigureComponent<UnitOfWorkManager>(DependencyLifecycle.SingleInstance);

            DI.Current.Register<ICommandContextProvider, IBus>(bus => new CommandContextProvider(bus));
            DI.Current.Register<IEventMapper, IMessageMapper>(messageMapper => new MessageMapperAdapter(messageMapper));
            DI.Current.Register<IEventFactory, IMessageCreator>(messageCreator => new MessageCreatorAdapter(messageCreator));
            DI.Current.Register(() => config.Builder.Build<IMessageMapper>());
            DI.Current.Register(() => config.Builder.Build<IMessageCreator>());
            DI.Current.Register<IEventPublisher, IBus>(bus => new BusAdapter(bus));
            DI.Current.Register(() => config.Builder.Build<IBus>());
            DI.Current.Register<IEventSerializer, IMessageSerializer>(messageSerializer => new MessageSerializerAdapter(messageSerializer));
            DI.Current.Register(() => config.Builder.Build<IMessageSerializer>());

            return config;
        }
    }
}