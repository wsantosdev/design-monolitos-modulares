using System.Reflection;
using WSantosDev.MonolitosModulares.Commons.Events;

namespace WSantosDev.MonolitosModulares.WebApi
{
    public static class EventBusExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services) 
        {
            return services.AddSingleton<IEventBus, InMemoryEventBus>();
        }

        public static IApplicationBuilder UseEventBus(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            var handlerTypes = GetHandlerTypes();
            for (var i = 0; i < handlerTypes.Length; i++)
            {
                var handler = scope.ServiceProvider.GetRequiredService(handlerTypes[i]);
                eventBus.Subscribe((IEventHandler)handler);
            }

            return app;
        }

        private static Type[] GetHandlerTypes()
        {
            return
            Assembly.GetExecutingAssembly()
                    .DefinedTypes
                    .Where(type => 
                        type.ImplementedInterfaces.Any(i => 
                            i.IsGenericType 
                            && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
                    .ToArray();
        }
    }
}
