using BSDigital.DTO;
using BSDigital.Helpers;

namespace BSDigital.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void MapLevels_SortsAscending()
        {
            var raw = new List<List<string>>
            {
                new() { "30000", "0.5" },
                new() { "29000", "1.0" }
            };

            var result = OrderBookMapper.MapLevels(raw, false);

            Assert.That(result[0].Price, Is.EqualTo(29000m));
            Assert.That(result[1].Price, Is.EqualTo(30000m));
        }

        [Test]
        public void MapLevels_SortsDescending()
        {
            var raw = new List<List<string>>
            {
                new() { "25000", "0.5" },
                new() { "29000", "1.0" }
            };

            var result = OrderBookMapper.MapLevels(raw, true);

            Assert.That(result[0].Price, Is.EqualTo(29000m));
            Assert.That(result[1].Price, Is.EqualTo(25000m));
        }

        [Test]
        public void CalculateCumulativeDepth_WithMultipleLevels_ReturnsCorrectCumulative()
        {
            var levels = new List<OrderBookItem>
            {
                new OrderBookItem { Price = 100m, Amount = 1m },
                new OrderBookItem { Price = 101m, Amount = 2m },
                new OrderBookItem { Price = 102m, Amount = 3m }
            };

            var result = OrderBookMapper.CalculateCumulativeDepth(levels);

            Assert.That(result.Count, Is.EqualTo(3));

            Assert.That(result[0].Price, Is.EqualTo(100m));
            Assert.That(result[0].Cumulative, Is.EqualTo(1m));

            Assert.That(result[1].Price, Is.EqualTo(101m));
            Assert.That(result[1].Cumulative, Is.EqualTo(3m));

            Assert.That(result[2].Price, Is.EqualTo(102m));
            Assert.That(result[2].Cumulative, Is.EqualTo(6m));
        }

        [Test]
        public void CalculateCumulativeDepth_WithEmptyList_ReturnsEmptyList()
        {
            var levels = new List<OrderBookItem>();
            var result = OrderBookMapper.CalculateCumulativeDepth(levels);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void CalculateCumulativeDepth_WithSingleLevel_ReturnsSameAmount()
        {
            var levels = new List<OrderBookItem>
            {
                new OrderBookItem { Price = 100m, Amount = 5m }
            };

            var result = OrderBookMapper.CalculateCumulativeDepth(levels);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Price, Is.EqualTo(100m));
            Assert.That(result[0].Cumulative, Is.EqualTo(5m));
        }

        [Test]
        public void ReturnsNull_WhenListIsEmpty()
        {
            var result = QuoteHelper.CalculateQuoteFromCumulative(
                new List<DepthPoint>(),
                1m
            );

            Assert.That(result, Is.Null);
        }

        [Test]
        public void ReturnsNull_WhenBtcAmountIsZeroOrNegative()
        {
            var asks = new List<DepthPoint>
            {
                new DepthPoint { Price = 100m, Cumulative = 1m }
            };

            Assert.That(QuoteHelper.CalculateQuoteFromCumulative(asks, 0m), Is.Null);
            Assert.That(QuoteHelper.CalculateQuoteFromCumulative(asks, -1m), Is.Null);
        }

        [Test]
        public void CalculatesCorrectly_WhenSingleLevelIsEnough()
        {
            var asks = new List<DepthPoint>
            {
                new DepthPoint { Price = 100m, Cumulative = 2m }
            };

            var result = QuoteHelper.CalculateQuoteFromCumulative(asks, 1.5m);

            Assert.That(result, Is.EqualTo(150m));
        }

        [Test]
        public void ReturnsNull_WhenNotEnoughLiquidity()
        {
            var asks = new List<DepthPoint>
            {
                new DepthPoint { Price = 100m, Cumulative = 1m },
                new DepthPoint { Price = 110m, Cumulative = 2m }
            };

            var result = QuoteHelper.CalculateQuoteFromCumulative(asks, 3m);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void HandlesExactCumulativeMatch()
        {
            var asks = new List<DepthPoint>
            {
                new DepthPoint { Price = 100m, Cumulative = 1m },
                new DepthPoint { Price = 110m, Cumulative = 2m }
            };

            var result = QuoteHelper.CalculateQuoteFromCumulative(asks, 2m);

            Assert.That(result, Is.EqualTo(210m));
        }
    }
}
