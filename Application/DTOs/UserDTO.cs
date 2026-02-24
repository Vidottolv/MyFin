using MyFin.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyFin.Application.DTOs
{
    public class UserDTO
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime DtCreation { get; set; } = DateTime.UtcNow;
        public int AccountsCount { get; set; }
        public int CategoriesCount { get; set; }
        public int TransactionsCount { get; set; }
    }

    public class CreateUserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
