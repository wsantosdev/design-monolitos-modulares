using WSantosDev.MonolitosModulares.Accounts;

namespace WSantosDev.MonolitosModulares.WebApi.Accounts
{
    public static class AccountsModuleExtensions
    {
        public static IServiceCollection AddAccountsModule(this IServiceCollection services)
        {
            return services.AddSingleton<IAccountStore, AccountStore>()
                           .AddTransient<IAccountService, AccountService>()
                           .AddTransient<OrderCreatedEventHandler>()
                           .AddTransient<OrderCanceledEventHandler>()
                           .AddTransient<ExchangeExecutedEventHandler>();
        }
    }
}
