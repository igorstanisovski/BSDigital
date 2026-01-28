namespace BSDigital.DTO
{
    public class DepthSnapshot
    {
        public List<DepthPoint> Bids { get; set; } = new();
        public List<DepthPoint> Asks { get; set; } = new();
        public DateTime Timestamp { get; set; }
    }
}
