using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WSantosDev.MonolitosModulares.WebApi.Swagger
{
    public static class SwaggerControllerOrderDiscover
    {
        public static string GetControllerOrder(ApiDescription apiDescription)
        {
            var orderAttribute = apiDescription.CustomAttributes()
                                               .OfType<SwaggerControllerOrderAttribute>()
                                               .FirstOrDefault();
            if (orderAttribute == default)
                return string.Empty;

            return orderAttribute.Order.ToString();
        }
    }
}
