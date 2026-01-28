using BSDigital.Entities;

namespace BSDigital.Interfaces
{
    public interface IBitstampService
    {
        Task StartAsync();
        Task StopAsync();
        Task SetUserBtcAmount(string connectionId, decimal btcAmount);
    }
}
