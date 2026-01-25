using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BSDigital.Entities
{
    [Table("ORDER_BOOK_SNAPSHOT")]
    public class OrderBookSnapshotEntity
    {
        [Key]
        [Column("ORDER_BOOK_SNAPSHOT_ID")]
        public long Id { get; set; }
        [Column("CREATED_ON")]
        public DateTime CreatedOn { get; set; }
        [Column("CODE")]
        public string Code { get; set; } = default!;

        [Column("BODY", TypeName = "jsonb")]
        public string Body { get; set; } = default!;
    }
}
