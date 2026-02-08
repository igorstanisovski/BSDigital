using BSDigital.API;
using BSDigital.API.Interfaces;
using BSDigital.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BSDigital.Controllers
{
    [ApiController]
    [Route(ApiEndpoints.OrderBook)]
    public class OrderBookController
    {
        private readonly IOrderBookApiService _orderBookApiService;

        public OrderBookController(IOrderBookApiService orderBookApiService)
        {
            _orderBookApiService = orderBookApiService;
        }

        [HttpGet]
        [Route("historical-data")]
        public async Task<DepthSnapshot> GetHistoricalData([FromQuery] string code, [FromQuery] DateTime timestamp)
        {
            var response = await _orderBookApiService.FetchHistoricalData(code, timestamp);
            return response;
        }
    }
}
