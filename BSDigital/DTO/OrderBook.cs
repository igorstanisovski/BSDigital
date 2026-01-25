using Newtonsoft.Json;

namespace BSDigital.DTO
{
    public class OrderBook
    {
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        [JsonProperty("bids")]
        public List<DepthPoint> Bids { get; set; } = new();
        [JsonProperty("asks")]
        public List<DepthPoint> Asks { get; set; } = new();
        public OrderBook(long timestamp, List<DepthPoint> bids, List<DepthPoint> asks)
        {
            Timestamp = timestamp;
            Bids = bids;
            Asks = asks;
        }
    }
}
