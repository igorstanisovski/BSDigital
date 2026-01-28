using BSDigital.Entities;
using Microsoft.EntityFrameworkCore;

namespace BSDigital.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<OrderBookSnapshotEntity> OrderBookSnapshots => Set<OrderBookSnapshotEntity>();
    }
}
