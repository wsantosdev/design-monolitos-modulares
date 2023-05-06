using WSantosDev.MonolitosModulares.Orders;

namespace WSantosDev.MonolitosModulares.WebApi.Orders
{
    public static class OrdersModuleExtensions
    {
        public static IServiceCollection AddOrdersModule(this IServiceCollection services)
        {
            return 
            services.AddSingleton<IOrderStore, OrderStore>()
                    .AddTransient<IOrderService, OrderService>()
                    .AddTransient<ExchangeExecutedEventHandler>();
        }
    }
}
