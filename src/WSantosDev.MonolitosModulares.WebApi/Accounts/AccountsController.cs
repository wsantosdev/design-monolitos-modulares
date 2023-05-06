using Microsoft.AspNetCore.Mvc;
using WSantosDev.MonolitosModulares.Accounts;

namespace WSantosDev.MonolitosModulares.WebApi.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerControllerOrder(1)]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService) =>
            _accountService = accountService;

        [HttpGet("Balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Balance()
        {
            var balance = _accountService.GetBalance(Constants.DefaultAccountId);
            return Ok(balance);
        }

        [HttpPost("Credit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Credit(decimal amount)
        {
            var creditResult = _accountService.Credit(Constants.DefaultAccountId, amount);
            if(creditResult)
                return Ok();

            if (creditResult.Error is InvalidAmountError)
                return BadRequest($"Valor para crédito inválido: {amount:C}");

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
