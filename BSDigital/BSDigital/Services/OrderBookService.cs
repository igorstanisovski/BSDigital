using BSDigital.DTO;
using BSDigital.Interfaces;
using System.Threading.Tasks;

namespace BSDigital.Services
{
    public class OrderBookService : IOrderBookService
    {
        private readonly object _lock = new();
        private List<DepthPoint> _bids = new();
        private List<DepthPoint> _asks = new();

        public event Action? OnUpdated;

        public List<DepthPoint> GetAsksSnapshot()
        {
            lock (_lock)
            {
                return _asks.ToList();
            }
        }

        public List<DepthPoint> GetBidsSnapshot()
        {
            lock (_lock)
            {
                return _bids.ToList();
            }
        }

        public void UpdateOrderBook(List<DepthPoint> bids, List<DepthPoint> asks)
        {
            lock (_lock)
            {
                _bids = bids;
                _asks = asks;
            }
            OnUpdated?.Invoke();
        }
    }
}
