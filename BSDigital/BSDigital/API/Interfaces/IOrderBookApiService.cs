using BSDigital.DTO;

namespace BSDigital.API.Interfaces
{
    public interface IOrderBookApiService
    {
        Task<DepthSnapshot> FetchHistoricalData(string code, DateTime timestamp);
    }
}
