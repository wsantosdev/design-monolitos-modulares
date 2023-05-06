using Microsoft.AspNetCore.Mvc;
using WSantosDev.MonolitosModulares.Portfolios;

namespace WSantosDev.MonolitosModulares.WebApi.Portfolios
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerControllerOrder(4)]
    public class PortfoliosController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;

        public PortfoliosController(IPortfolioService custodyService)
        {
            _portfolioService = custodyService;
        }

        [HttpGet]
        public IActionResult Get() 
        {
            var portfolio = _portfolioService.GetByAccount(Constants.DefaultAccountId);
            return Ok(portfolio.Entries.Select(PortfolioEntryViewModel.From));
        }
    }
}
