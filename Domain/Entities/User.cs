using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace MyFin.Domain.Entities
{
    public class TBLUser
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();
        [Required]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public DateTime DtCreation { get; set; } = DateTime.UtcNow;
        public ICollection<TBLAccount> Accounts { get; set; } = new List<TBLAccount>();
        public ICollection<TBLCategory> Categories { get; set; } = new List<TBLCategory>();
        public ICollection<TBLTransaction> Transactions { get; set; } = new List<TBLTransaction>();
    }
}
