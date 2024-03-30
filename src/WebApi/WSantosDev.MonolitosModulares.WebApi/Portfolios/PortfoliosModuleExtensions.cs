using WSantosDev.MonolitosModulares.Portfolios;

namespace WSantosDev.MonolitosModulares.WebApi.Portfolios
{
    public static class PortfoliosModuleExtensions
    {
        public static IServiceCollection AddPortfoliosModule(this IServiceCollection services)
        {
            return services.AddSingleton<IPortfolioStore, PortfolioStore>()
                           .AddTransient<IPortfolioService, PortfolioService>()
                           .AddTransient<OrderCreatedEventHandler>()
                           .AddTransient<ExchangeExecutedEventHandler>()
                           .AddTransient<OrderCanceledEventHandler>();
        }
    }
}
