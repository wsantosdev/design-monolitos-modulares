using WSantosDev.MonolitosModulares.WebApi;
using WSantosDev.MonolitosModulares.WebApi.Accounts;
using WSantosDev.MonolitosModulares.WebApi.Exchange;
using WSantosDev.MonolitosModulares.WebApi.Orders;
using WSantosDev.MonolitosModulares.WebApi.Portfolios;
using WSantosDev.MonolitosModulares.WebApi.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAccountsModule();
builder.Services.AddPortfoliosModule();
builder.Services.AddOrdersModule();
builder.Services.AddExchangeModule();
builder.Services.AddEventBus();

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddHostedService<DefaultHostedService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup => 
    setup.OrderActionsBy(apiDesc => 
        SwaggerControllerOrderDiscover.GetControllerOrder(apiDesc)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.DefaultModelsExpandDepth(-1));
}

app.UseHttpsRedirection();
app.UseEventBus();
app.MapControllers();

app.Run();
