using MyFin.Domain.Entities;

namespace MyFin.Application.DTOs
{
    public class CategoriesDTO
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid? UserId { get; set; }
        public TBLUser? User { get; set; }
    }

    public class CreateCategoryDTO
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class UpdateCategoryDTO
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
