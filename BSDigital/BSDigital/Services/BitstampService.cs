using BSDigital.DTO;
using BSDigital.Entities;
using BSDigital.Helpers;
using BSDigital.Hubs;
using BSDigital.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace BSDigital.Services
{
    public class BitstampService : IBitstampService
    {
        private readonly IBitstampClient _client;
        private readonly IAuditService _auditService;
        private readonly IOrderBookService _orderBookService;
        private readonly IHubContext<MarketDataHub> _hubContext;

        private readonly Dictionary<string, decimal> _btcAmounts = new();
        private DateTime _lastSent = DateTime.MinValue;

        public BitstampService(IBitstampClient client, IHubContext<MarketDataHub> hubContext, IAuditService auditService, IOrderBookService orderBookService)
        {
            _orderBookService = orderBookService;
            _client = client;
            _client.OnMessage += async (json) =>
            {
                try
                {
                    await HandleMessage(json);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };

            _orderBookService.OnUpdated += async () => await CalculateQuoteAsync();
            _hubContext = hubContext;
            _auditService = auditService;
        }

        public async Task StartAsync()
        {
            await _client.ConnectAsync();
        }

        public async Task StopAsync()
        {
            await _client.DisconnectAsync();
        }

        public async Task SetUserBtcAmount(string connectionId, decimal btcAmount)
        {
            if(btcAmount == 0)
            {
                _btcAmounts.Remove(connectionId);
            } else
            {
               _btcAmounts[connectionId] = btcAmount;
            }
        }

        private async Task HandleMessage(string json)
        {
            BitstampMessage? message;

            try
            {
                message = JsonConvert.DeserializeObject<BitstampMessage>(json);
            }
            catch (JsonException ex)
            {
                return;
            }

            if (message == null || message.Event != "data" || message.Data == null) 
            {
                return; 
            }

            var bids = OrderBookMapper.MapLevels(message.Data.Bids, true);
            var asks = OrderBookMapper.MapLevels(message.Data.Asks, false);

            var bidsCumulativeDepth = OrderBookMapper.CalculateCumulativeDepth(bids);
            var asksCumulativeDepth = OrderBookMapper.CalculateCumulativeDepth(asks);

            var orderBook = new OrderBook(message.Data.Timestamp, bidsCumulativeDepth, asksCumulativeDepth);

            if (ShouldPublish())
            {
                _lastSent = DateTime.UtcNow;

                await PublishDepthAsync(bidsCumulativeDepth, asksCumulativeDepth);

                _orderBookService.UpdateOrderBook(bidsCumulativeDepth, asksCumulativeDepth);

                var entity = new OrderBookSnapshotEntity
                {
                    CreatedOn = DateTime.UtcNow,
                    Code = "BTC/EUR",
                    Body = JsonConvert.SerializeObject(orderBook, Formatting.Indented)
                };

                _auditService.LogSnapshotAsync(entity);
            }
        }

        private bool ShouldPublish()
        {
            return (DateTime.UtcNow - _lastSent) > TimeSpan.FromMilliseconds(1000);
        }

        private async Task PublishDepthAsync(List<DepthPoint> bidsDepth, List<DepthPoint> asksDepth)
        {
            var dto = new DepthSnapshot
            {
                Bids = bidsDepth,
                Asks = asksDepth,
                Timestamp = DateTime.UtcNow
            };

            await _hubContext.Clients.All.SendAsync("DepthUpdate", dto);
        }

        private async Task CalculateQuoteAsync()
        {
            foreach (var kvp in _btcAmounts)
            {
                var connectionId = kvp.Key;
                var btcAmount = kvp.Value;
                var quote = QuoteHelper.CalculateQuoteFromCumulative(_orderBookService.GetAsksSnapshot(), btcAmount);
                await _hubContext.Clients.Client(connectionId).SendAsync("QuoteUpdated", quote);
            }
        }
    }
}
