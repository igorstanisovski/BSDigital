using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BSDigital.Entities
{
    [Table("ORDER_BOOK_SNAPSHOT")]
    [Index(nameof(CreatedOn), IsUnique = true)]
    public class OrderBookSnapshotEntity
    {
        [Key]
        [Column("ORDER_BOOK_SNAPSHOT_ID")]
        public long Id { get; set; }

        [Column("CREATED_ON")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Column("CODE")]
        [Required]
        public string Code { get; set; } = default!;

        [Column("BODY", TypeName = "jsonb")]
        [Required]
        public string Body { get; set; } = default!;
    }
}
