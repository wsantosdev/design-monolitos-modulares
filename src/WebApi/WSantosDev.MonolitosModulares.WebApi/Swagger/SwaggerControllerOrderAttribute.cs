namespace WSantosDev.MonolitosModulares.WebApi
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SwaggerControllerOrderAttribute : Attribute
    {
        public uint Order { get; private set; }

        public SwaggerControllerOrderAttribute(uint order)
        {
            Order = order;
        }
    }
}
