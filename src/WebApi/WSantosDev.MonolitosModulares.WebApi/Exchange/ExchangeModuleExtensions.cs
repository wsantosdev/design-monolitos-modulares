using WSantosDev.MonolitosModulares.Exchange;

namespace WSantosDev.MonolitosModulares.WebApi.Exchange
{
    public static class ExchangeModuleExtensions
    {
        public static IServiceCollection AddExchangeModule(this IServiceCollection services) 
        {
            return services.AddSingleton<IOrderStore, OrderStore>()
                           .AddTransient<IExchangeService, ExchangeService>()
                           .AddTransient<OrderCreatedEventHandler>()
                           .AddTransient<OrderCanceledEventHandler>();
        }
    }
}
