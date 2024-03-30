using WSantosDev.MonolitosModulares.Accounts;
using WSantosDev.MonolitosModulares.Portfolios;

namespace WSantosDev.MonolitosModulares.WebApi
{
    public sealed class DefaultHostedService : IHostedService
    {
        private readonly IAccountService _accountService;
        private readonly IPortfolioService _portfolioService;

        public DefaultHostedService(IAccountService accountService,
                                    IPortfolioService portfolioService)
        {
            _accountService = accountService;
            _portfolioService = portfolioService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _accountService.Create(Constants.DefaultAccountId, 1_000_000m);
            _portfolioService.Create(Constants.DefaultAccountId);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
