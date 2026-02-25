using MyFin.Domain;

namespace MyFin.Application.DTOs
{
    public class TransactionDTO
    {
        public Guid TransactionId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime DtTimeStamp { get; set; }
        public TransactionType Type { get; set; }
        public Guid AccountId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid UserId { get; set; }
    }

    public class CreateTransactionDTO
    {
        public Guid TransactionId { get; set; }
        public TransactionType Type { get; set; }
        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }
        public Guid? CategoryId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }

    public class UpdateTransactionDTO
    {
        public Guid TransactionId { get; set; }
        public TransactionType Type { get; set; }
        public Guid UserId { get; set; }
    }
}
