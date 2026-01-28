using Newtonsoft.Json;

namespace BSDigital.DTO
{
    public class BitstampData
    {
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("bids")]
        public List<List<string>> Bids { get; set; } = new();

        [JsonProperty("asks")]
        public List<List<string>> Asks { get; set; } = new();
    }
}
