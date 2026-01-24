namespace BSDigital.Entities
{
    public class OrderBook
    {
        private long Timestamp { get; set; }
        private List<OrderBookItem> Bids { get; set; } = new();
        private List<OrderBookItem> Asks { get; set; } = new();
        public OrderBook(long timestamp, List<OrderBookItem> bids, List<OrderBookItem> asks)
        {
            Timestamp = timestamp;
            Bids = bids;
            Asks = asks;
        }
    }
}
