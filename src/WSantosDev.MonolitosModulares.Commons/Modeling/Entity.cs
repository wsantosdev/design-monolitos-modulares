namespace WSantosDev.MonolitosModulares.Commons.Modeling
{
    public abstract class Entity<TId>
    {
        public virtual TId Id { get; protected init; }

        protected Entity(TId id) =>
            Id = id;

        public override bool Equals(object? obj)
        {
            if(obj is null || obj is not Entity<TId> other)
                return false;

            return Id!.Equals(other.Id);
        }

        public override int GetHashCode() =>
            HashCode.Combine(Id);

        public static bool operator == (Entity<TId> left, Entity<TId> right) =>
            left.Equals(right);

        public static bool operator !=(Entity<TId> left, Entity<TId> right) =>
            !left.Equals(right);
    }
}
