using BSDigital.DTO;

namespace BSDigital.Helpers
{
    public static class QuoteHelper
    {
        public static decimal? CalculateQuoteFromCumulative(List<DepthPoint> cumulativeAsks, decimal btcAmount)
        {
            if (cumulativeAsks == null || cumulativeAsks.Count == 0 || btcAmount <= 0)
                return null;

            decimal remaining = btcAmount;
            decimal totalCost = 0;
            decimal prevCumulative = 0;

            foreach (var ask in cumulativeAsks)
            {
                decimal availableAtLevel = ask.Cumulative - prevCumulative;
                decimal take = Math.Min(remaining, availableAtLevel);

                totalCost += take * ask.Price;
                remaining -= take;
                prevCumulative = ask.Cumulative;

                if (remaining <= 0) break;
            }

            return remaining <= 0 ? totalCost : null;
        }
    }
}
