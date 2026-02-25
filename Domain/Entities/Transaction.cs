using System.ComponentModel.DataAnnotations;

namespace MyFin.Domain.Entities
{
    public class TBLTransaction
    {
        [Key]
        public Guid TransactionId { get; set; } = Guid.NewGuid();

        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime DtTimeStamp { get; set; }
        public TransactionType Type { get; set; }

        // FKs
        public Guid AccountId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid UserId { get; set; }

        // Navigations
        public TBLAccount? Account { get; set; }
        public TBLCategory? Category { get; set; }
        public TBLUser? User { get; set; }
    }
}
