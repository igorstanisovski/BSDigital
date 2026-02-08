using BSDigital.DataAccess;
using BSDigital.Entities;
using BSDigital.Interfaces;
using BSDigital.Queries;
using Microsoft.EntityFrameworkCore;
using System;

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
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.OrderBookSnapshots.AddRange(snapshots);
            await context.SaveChangesAsync();
        }

        public async Task<OrderBookSnapshotEntity?> GetSnapshotByTimestamp(string code, DateTime timestamp)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var snapshot = await context.Database.SqlQueryRaw<OrderBookSnapshotEntity>(OrderBookSnapshotQueries.GetHistoricalDataByTimestamp, code, timestamp).FirstOrDefaultAsync(); ;
            return snapshot;
        }
    }
}
