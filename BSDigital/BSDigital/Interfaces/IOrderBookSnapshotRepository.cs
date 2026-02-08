using BSDigital.Entities;

namespace BSDigital.Interfaces
{
    public interface IOrderBookSnapshotRepository
    {
        Task AddRangeAsync(IEnumerable<OrderBookSnapshotEntity> snapshots);
        Task<OrderBookSnapshotEntity?> GetSnapshotByTimestamp(string code, DateTime timestamp);  
    }
}
