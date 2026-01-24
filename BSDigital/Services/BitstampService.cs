using BSDigital.DTO;
using BSDigital.Entities;
using BSDigital.Hubs;
using BSDigital.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Globalization;
using System.Threading.Tasks;

namespace BSDigital.Services
{
    public class BitstampService
    {
        private readonly IBitstampClient _client;
        private readonly IHubContext<MarketDataHub> _hubContext;

        public BitstampService(IBitstampClient client, IHubContext<MarketDataHub> hubContext)
        {
            _client = client;
            _client.OnMessage += async (json) =>
            {
                try
                {
                    await HandleMessage(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in HandleMessage: {ex}");
                }
            };
            _hubContext = hubContext;
        }

        public async Task StartAsync()
        {
            await _client.ConnectAsync();
        }

        public async Task StopAsync()
        {
            await _client.DisconnectAsync();
        }

        private async Task HandleMessage(string json)
        {
            Console.WriteLine("Received something....");

            BitstampMessage? message;

            try
            {
                message = JsonConvert.DeserializeObject<BitstampMessage>(json);
            }
            catch (JsonException ex)
            {
                // malformed JSON → log and skip
                Console.WriteLine($"JSON parse error: {ex.Message}");
                return;
            }

            if (message == null || message.Event != "data" || message.Data == null) 
            {
                return; 
            }

            var bids = MapLevels(message.Data.Bids, true);
            var asks = MapLevels(message.Data.Asks, false);

            var orderBook = new OrderBook(message.Data.Timestamp, bids, asks);

            var bidsCumulativeDepth = CalculateCumulativeDepth(bids);
            var asksCumulativeDepth = CalculateCumulativeDepth(asks);

            await PublishDepthAsync(bidsCumulativeDepth, asksCumulativeDepth);
        }

        private List<OrderBookItem> MapLevels(List<List<string>> rawLevels, bool descending)
        {
            var levels = rawLevels
                .Where(l => l.Count == 2)
                .Select(l => new OrderBookItem
                {
                    Price = decimal.Parse(l[0], CultureInfo.InvariantCulture),
                    Amount = decimal.Parse(l[1], CultureInfo.InvariantCulture)
                });

            return descending
                ? levels.OrderByDescending(l => l.Price).ToList()
                : levels.OrderBy(l => l.Price).ToList();
        }

        private List<DepthPoint> CalculateCumulativeDepth(List<OrderBookItem> l)
        {
            var result = new List<DepthPoint>();
            decimal sum = 0;

            foreach (var i in l)
            {
                sum += i.Amount;
                result.Add(new DepthPoint
                {
                    Price = i.Price,
                    Cumulative = sum
                });
            }

            return result;
        }

        private async Task PublishDepthAsync(List<DepthPoint> bidsDepth, List<DepthPoint> asksDepth)
        {
            var dto = new DepthSnapshot
            {
                Bids = bidsDepth.Select(b => new DepthPoint
                {
                    Price = b.Price,
                    Cumulative = b.Cumulative
                }).ToList(),

                Asks = asksDepth.Select(a => new DepthPoint
                {
                    Price = a.Price,
                    Cumulative = a.Cumulative
                }).ToList(),

                Timestamp = DateTime.UtcNow
            };

            await _hubContext.Clients.All.SendAsync("DepthUpdate", dto);
        }
    }
}
