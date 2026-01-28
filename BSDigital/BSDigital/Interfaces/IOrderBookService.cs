using BSDigital.DTO;

namespace BSDigital.Interfaces
{
    public interface IOrderBookService
    {
        List<DepthPoint> GetAsksSnapshot();
        List<DepthPoint> GetBidsSnapshot();
        void UpdateOrderBook(List<DepthPoint> bids, List<DepthPoint> asks);

        event Action? OnUpdated;
    }
}
