using BSDigital.Entities;

namespace BSDigital.Interfaces
{
    public interface IOrderBookSnapshotRepository
    {
        Task AddRangeAsync(IEnumerable<OrderBookSnapshotEntity> snapshots);
    }
}
