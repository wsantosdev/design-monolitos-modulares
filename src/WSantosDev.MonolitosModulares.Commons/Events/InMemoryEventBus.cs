namespace WSantosDev.MonolitosModulares.Commons.Events
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly Dictionary<Type, IList<IEventHandler>> _subscribedHandlers = new();

        public void Subscribe(IEventHandler eventHandler)
        {
            var eventType = GetEventType(eventHandler);
            if (_subscribedHandlers.TryGetValue(eventType, out var handlersList))
            {
                handlersList.Add(eventHandler);
                return;
            }

            handlersList = new List<IEventHandler>() { eventHandler };
            _subscribedHandlers.Add(eventType, handlersList);
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (!_subscribedHandlers.TryGetValue(typeof(TEvent), out var handlersList))
                return;

            for (var i = 0; i < handlersList.Count; i++)
                ((IEventHandler<TEvent>)handlersList[i]).Handle(@event);
        }

        private static Type GetEventType(object eventHandler)
        {
            return eventHandler.GetType()
                               .GetInterfaces()
                               .First(i => i.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                               .GenericTypeArguments[0];
        }
    }
}
