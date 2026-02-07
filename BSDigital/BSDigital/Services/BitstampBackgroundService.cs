
using BSDigital.Interfaces;

namespace BSDigital.Services
{
    public class BitstampBackgroundService : BackgroundService
    {
        private readonly IBitstampService _bitstampService;

        public BitstampBackgroundService(IBitstampService bitstampService)
        {
            _bitstampService = bitstampService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {   
            await _bitstampService.StartAsync();
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _bitstampService.StopAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
