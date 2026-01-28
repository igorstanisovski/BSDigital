using BSDigital.Entities;

namespace BSDigital.Interfaces
{
    public interface IAuditService
    {
        void LogSnapshotAsync(OrderBookSnapshotEntity orderBookSnapshotEntity);
    }
}
