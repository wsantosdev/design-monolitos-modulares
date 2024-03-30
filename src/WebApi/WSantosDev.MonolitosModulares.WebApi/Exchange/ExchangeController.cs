using Microsoft.AspNetCore.Mvc;
using WSantosDev.MonolitosModulares.Exchange;

namespace WSantosDev.MonolitosModulares.WebApi.Exchange
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerControllerOrder(3)]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;

        public ExchangeController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        [HttpGet("Orders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll()
        {
            var orders = _exchangeService.GetAll();
            return Ok(orders.Select(OrderViewModel.From));
        }

        [HttpPost("Execute")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Execute(Guid orderId) 
        {
            var executeResult = _exchangeService.Execute(orderId);
            if(executeResult) 
                return Ok();

            return executeResult.Error switch
            {
                AlreadyFilledError => Conflict("Ordem já executada."),
                AlreadyCanceledError => Conflict("Ordem já cancelada."),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Falha não especificada.")
            };
        }
    }
}
