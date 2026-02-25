using System.ComponentModel.DataAnnotations;

namespace MyFin.Domain.Entities
{
    public class TBLCategory
    {
        [Key]
        public Guid CategoryId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid? UserId { get; set; }
        public TBLUser? User { get; set; }
    }
}
