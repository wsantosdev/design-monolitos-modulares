namespace WSantosDev.MonolitosModulares.Commons.Modeling
{
    internal interface ISimpleValue { }

    public class SimpleValue<T> : IEquatable<T>, ISimpleValue
    {
        public T Value { get; private init; }

        protected SimpleValue(T value) =>
            Value = value;

        public static bool operator ==(SimpleValue<T> left, SimpleValue<T> right) =>
            left.Equals(right);

        public static bool operator !=(SimpleValue<T> left, SimpleValue<T> right) =>
            !left.Equals(right);

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not SimpleValue<T> simpleValue)
                return false;

            return Equals(simpleValue.Value);
        }

        public bool Equals(T? otherValue)
        {
            if (otherValue is null)
                return false;

            return Value!.Equals(otherValue);
        }

        public override int GetHashCode() =>
            HashCode.Combine(Value);

        public override string ToString() =>
            Value!.ToString()!;
    }
}