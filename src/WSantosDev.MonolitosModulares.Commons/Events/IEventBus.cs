namespace WSantosDev.MonolitosModulares.Commons.Events
{
    public interface IEventBus
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IEvent;
        void Subscribe(IEventHandler eventHandler);
    }
}
