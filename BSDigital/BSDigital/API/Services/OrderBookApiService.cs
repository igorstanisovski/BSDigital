using BSDigital.API.Interfaces;
using BSDigital.DTO;
using BSDigital.Interfaces;
using Newtonsoft.Json;
using System.Reactive;

namespace BSDigital.API.Services
{
    public class OrderBookApiService : IOrderBookApiService
    {
        private readonly IOrderBookSnapshotRepository _snapshotRepository;

        public OrderBookApiService(IOrderBookSnapshotRepository snapshotRepository)
        {
            _snapshotRepository = snapshotRepository;
        }

        public async Task<DepthSnapshot> FetchHistoricalData(string code, DateTime dateTime)
        {
            var snapshot = await _snapshotRepository.GetSnapshotByTimestamp(code, dateTime);

            if (snapshot == null)
            {
                throw new Exception("Not found!");
            }

            var orderBook = JsonConvert.DeserializeObject<OrderBook>(snapshot.Body);

            if (orderBook == null)
            {
                throw new InvalidOperationException($"Failed to deserialize order book data for snapshot {snapshot.Id}");
            }

            return new DepthSnapshot
            {
                Timestamp = DateTimeOffset.FromUnixTimeSeconds(orderBook.Timestamp).UtcDateTime,
                Asks = orderBook.Asks,
                Bids = orderBook.Bids
            };
        }
    }
}
