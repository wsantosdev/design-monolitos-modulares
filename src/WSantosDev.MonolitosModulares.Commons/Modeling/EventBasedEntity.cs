using WSantosDev.MonolitosModulares.Commons.Events;

namespace WSantosDev.MonolitosModulares.Commons.Modeling
{
    public abstract class EventBasedEntity<TId> : Entity<TId>
    {
        private readonly Queue<IEvent> _pendingEvents = new();

        public IReadOnlyCollection<IEvent> PendingEvents =>
            _pendingEvents.ToList().AsReadOnly();

        protected EventBasedEntity(TId id) : base(id) { }

        protected void RaiseEvent<TEvent>(TEvent pendingEvent) where TEvent : IEvent
        {
            _pendingEvents.Enqueue(pendingEvent);
            Apply(pendingEvent);
        }

        protected abstract void Apply(IEvent pendingEvent);

        public void Commit() =>
            _pendingEvents.Clear();
    }
}
