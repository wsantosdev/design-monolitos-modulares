using Microsoft.AspNetCore.Mvc;
using WSantosDev.MonolitosModulares.Orders;
using WSantosDev.MonolitosModulares.Shared;
using Account = WSantosDev.MonolitosModulares.Accounts;
using Portifolio = WSantosDev.MonolitosModulares.Portfolios;

namespace WSantosDev.MonolitosModulares.WebApi.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerControllerOrder(2)]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly Account.IAccountService _accountService;
        private readonly Portifolio.IPortfolioService _portfolioService;

        public OrdersController(IOrderService orderService, 
                                Account.IAccountService accountService,
                                Portifolio.IPortfolioService portfolioService)
        {
            _orderService = orderService;
            _accountService = accountService;
            _portfolioService = portfolioService;
        }

        [HttpPost("Send")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Send(SendOrderRequest request)
        {
            if (request.Side == OrderSide.Buy)
            {
                var balance = _accountService.GetBalance(Constants.DefaultAccountId);
                if (balance < (request.Quantity * request.Price))
                    return Conflict("Não há recursos disponíveis para esta operação.");
            }
            
            if(request.Side == OrderSide.Sell)
            {
                var entry = _portfolioService.GetEntryBySymbol(Constants.DefaultAccountId, request.Symbol);
                if(entry.Quantity < request.Quantity)
                    return Conflict("Não há ativos disponiveis para esta operação.");
            }

            var sendResult = _orderService.Send(Constants.DefaultAccountId, request.Side, 
                                                request.Quantity, request.Symbol, request.Price);
            if (!sendResult)
                return sendResult.Error switch
                {
                    InvalidSideError => BadRequest("Tipo de operação inválido. Valores permitidos: 'Buy' ou 'Sell'."),
                    InvalidQuantityError => BadRequest("Quantidade inválida. A quantidade deve ser maior que 0."),
                    InvalidSymbolError => BadRequest("Símbolo inválido. O símbolo deve ser informado."),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, "Falha não especificada.")
                };

            return Created(sendResult.Value.Id.ToString(), OrderViewModel.From(sendResult.Value));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult List()
        {
            var orders = _orderService.GetAllByAccount(Constants.DefaultAccountId);
            var viewModelList = orders.Select(OrderViewModel.From);

            return Ok(viewModelList);
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get(Guid orderId)
        {
            var order = _orderService.GetById(orderId);
            
            return Ok(order);
        }

        [HttpPost("Cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Cancel(Guid orderId)
        {
            var cancelResult = _orderService.Cancel(orderId);
            if (!cancelResult)
                return cancelResult.Error switch
                {
                    AlreadyCanceledError => BadRequest("Ordem previamente cancelada."),
                    AlreadyFilledError => BadRequest("Ordem executada. Ordens executadas não podem ser canceladas."),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, "Falha não especificada.")
                };

            return Ok();
        }
    }
}
