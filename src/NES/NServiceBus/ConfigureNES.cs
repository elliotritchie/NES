using NServiceBus;
using NServiceBus.MessageInterfaces;
using NServiceBus.ObjectBuilder;
using NServiceBus.Serialization;

namespace NES.NServiceBus
{
    public static class ConfigureNES
    {
        public static Configure NES(this Configure config)
        {
            config.Configurer.ConfigureComponent<MessageModule>(ComponentCallModelEnum.Singlecall);

            DI.Current.Register<IEventFactory, IMessageMapper>(messageMapper => new MessageMapperAdapter(messageMapper));
            DI.Current.Register(() => config.Builder.Build<IMessageMapper>());
            DI.Current.Register<IEventPublisher, IBus>(bus => new BusAdapter(bus));
            DI.Current.Register(() => config.Builder.Build<IBus>());
            DI.Current.Register<IEventSerializer, IMessageSerializer>(messageSerializer => new MessageSerializerAdapter(messageSerializer));
            DI.Current.Register(() => config.Builder.Build<IMessageSerializer>());

            return config;
        }
    }
}