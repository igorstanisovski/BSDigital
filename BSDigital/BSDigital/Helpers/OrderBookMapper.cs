using BSDigital.DTO;
using System.Globalization;

namespace BSDigital.Helpers
{
    public static class OrderBookMapper
    {
        public static List<OrderBookItem> MapLevels(List<List<string>> rawLevels, bool descending)
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

        public static List<DepthPoint> CalculateCumulativeDepth(List<OrderBookItem> levels)
        {
            var result = new List<DepthPoint>();
            decimal sum = 0;

            foreach (var i in levels)
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
    }
}
