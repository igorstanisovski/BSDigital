using BSDigital.DataAccess;
using BSDigital.Entities;
using BSDigital.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BSDigital.Repositories
{
    public class OrderBookSnapshotRepository : IOrderBookSnapshotRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public OrderBookSnapshotRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddRangeAsync(IEnumerable<OrderBookSnapshotEntity> snapshots)
        {
            await using var db = await _contextFactory.CreateDbContextAsync();
            db.OrderBookSnapshots.AddRange(snapshots);
            await db.SaveChangesAsync();
        }
    }
}
