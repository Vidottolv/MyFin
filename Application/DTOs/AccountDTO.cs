using MyFin.Domain;
using MyFin.Domain.Entities;

namespace MyFin.Application.DTOs
{
    public class AccountDTO
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal TotalTransactions { get; set; }
        public Guid UserId { get; set; }
        public TBLUser User { get; set; }
    }
    public class CreateAccountDTO
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal InitialBalance { get; set; }
        public Guid UserId { get; set; }
    }

    public class UpdateAccountDTO
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid UserId { get; set; }
        public decimal CurrentBalance { get; set; }
    }

    public class UpdateAccountBalanceDTO
    {
        public Guid UserId { get; set; }
        public decimal NewQty { get; set; }
        public TransactionType Type { get; set; }
    }
}
