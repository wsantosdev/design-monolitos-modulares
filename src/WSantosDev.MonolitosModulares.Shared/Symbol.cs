using WSantosDev.MonolitosModulares.Commons.Modeling;

namespace WSantosDev.MonolitosModulares.Shared
{
    public sealed class Symbol : SimpleValue<string>
    {
        public static readonly Symbol Empty = new(string.Empty);

        private Symbol(string value) : base(value) { }

        public static implicit operator Symbol(string value) =>
            new(value);

        public static implicit operator string(Symbol symbol) =>
            symbol.Value;

        public static bool operator ==(Symbol left, string right) =>
            left.Value == right;

        public static bool operator !=(Symbol left, string right) =>
            left.Value != right;

        public override bool Equals(object? obj) =>
            base.Equals(obj);

        public override int GetHashCode() =>
            base.GetHashCode();
    }
}
