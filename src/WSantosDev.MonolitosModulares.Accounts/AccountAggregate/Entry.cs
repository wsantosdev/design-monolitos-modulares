using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;

namespace WSantosDev.MonolitosModulares.Accounts
{
    public sealed class Entry : SimpleValue<decimal>
    {
        public static readonly Entry Empty = new(0);

        private Entry(decimal value) : base(value) { }

        public static Result<Entry> Credit(Money amount)
        {
            if(amount == Money.Zero)
                Result<Entry>.Fail(Errors.InvalidAmount);

            return new Entry(amount.Value);
        }

        public static Result<Entry> Debit(Money amount)
        {
            if (amount == Money.Zero)
                Result<Entry>.Fail(Errors.InvalidAmount);

            return new Entry(amount.Value * -1);
        }
    }
}
