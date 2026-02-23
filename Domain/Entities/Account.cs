using MyFin.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class TBLAccount
{
    [Key]
    public Guid AccountId { get; set; } = new Guid();

    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public Guid UserId { get; set; }
    public TBLUser User { get; set; }
    public ICollection<TBLTransaction> Transactions { get; set; } = new List<TBLTransaction>();
}
