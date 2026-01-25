using BSDigital.Entities;
using BSDigital.Interfaces;

namespace BSDigital.Services
{
    public class AuditService : IAuditService
    {
        private readonly IOrderBookSnapshotRepository _repo;
        private readonly List<OrderBookSnapshotEntity> _buffer = new();
        private readonly object _lock = new();

        private readonly int _batchSize;
        public AuditService(IOrderBookSnapshotRepository repo, int batchSize = 10)
        {
            _repo = repo;
            _batchSize = batchSize;
        }

        public void LogSnapshotAsync(OrderBookSnapshotEntity entity)
        {
            lock (_lock)
            {
                _buffer.Add(entity);

                if (_buffer.Count >= _batchSize)
                {
                    var batch = _buffer.ToList();
                    _buffer.Clear();

                    Task.Run(async () => await _repo.AddRangeAsync(batch));
                }
            }
        }
    }
}