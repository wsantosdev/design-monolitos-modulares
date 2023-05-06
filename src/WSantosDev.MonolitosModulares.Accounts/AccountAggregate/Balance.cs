using WSantosDev.MonolitosModulares.Commons.Modeling;

namespace WSantosDev.MonolitosModulares.Accounts
{
    public sealed class Balance : SimpleValue<decimal>
    {
        public static readonly Balance Zero = new (0);

        private Balance(decimal value) : base(value){ }

        public static implicit operator Balance(decimal value) =>
            new(value);

        public static bool operator >(Balance balance, Money amount) =>
            balance.Value > amount.Value;

        public static bool operator <(Balance balance, Money amount) =>
            balance.Value < amount.Value;
    }
}
