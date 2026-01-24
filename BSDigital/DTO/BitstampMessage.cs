using Newtonsoft.Json;

namespace BSDigital.DTO
{
    public class BitstampMessage
    {
        [JsonProperty("event")]
        public string Event { get; set; } = null!;

        [JsonProperty("channel")]
        public string Channel { get; set; } = null!;

        [JsonProperty("data")]
        public BitstampData Data { get; set; } = null!;
    }
}
