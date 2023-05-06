using WSantosDev.MonolitosModulares.Commons.Modeling;

namespace WSantosDev.MonolitosModulares.Shared
{
    public sealed class AccountId : SimpleValue<Guid>
    {
        public static readonly AccountId Empty = new(Guid.Empty);

        private AccountId(Guid value) : base(value) { }

        public static implicit operator AccountId(Guid value) =>
            new(value);
    }
}