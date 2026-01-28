using BSDigital.Helpers;
using BSDigital.Interfaces;
using BSDigital.Services;
using Microsoft.AspNetCore.SignalR;

namespace BSDigital.Hubs
{
    public class MarketDataHub : Hub
    {
        private readonly IBitstampService _bitstampService;

        public MarketDataHub(IBitstampService bitstampService)
        {
            _bitstampService = bitstampService;
        }

        public async Task SetBtcAmount(decimal btcAmount)
        {
            await _bitstampService.SetUserBtcAmount(Context.ConnectionId, btcAmount);
        }
    }
}
