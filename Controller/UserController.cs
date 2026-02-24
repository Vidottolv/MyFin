using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFin.Application.DTOs;
using MyFin.Domain.Entities;
using MyFin.Persistence;

namespace MyFin.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Accounts)
                .Include(u => u.Categories)
                .Include(u => u.Transactions)
                .ToListAsync();

            var result = users.Select(u => new UserDTO
            {
                UserId= u.UserId,
                Email = u.Email,
                Name = u.Name,
                DtCreation = u.DtCreation,
                AccountsCount = u.Accounts.Count,
                CategoriesCount = u.Categories.Count,
                TransactionsCount = u.Transactions.Count
            });

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDTO>> GetUser(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Accounts)
                .Include(u => u.Categories)
                .Include(u => u.Transactions)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return NotFound();

            var dto = new UserDTO 
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.Name,
                DtCreation = user.DtCreation,
                AccountsCount = user.Accounts.Count,
                CategoriesCount = user.Categories.Count,
                TransactionsCount = user.Transactions.Count
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO dto)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists)
                return Conflict("Já existe um usuário com esse e-mail.");

            var user = new TBLUser
            {
                UserId = Guid.NewGuid(),
                Email = dto.Email,
                Name = dto.Name,
                DtCreation = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = new UserDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.Name,
                DtCreation = user.DtCreation,
                AccountsCount = 0,
                CategoriesCount = 0,
                TransactionsCount = 0
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, TBLUser dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != user.Email)
            {
                var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
                if (exists)
                    return Conflict("Já existe um usuário com esse e-mail.");
                user.Email = dto.Email;
            }

            if (!string.IsNullOrWhiteSpace(dto.Name))
                user.Name = dto.Name;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Accounts)
                .Include(u => u.Categories)
                .Include(u => u.Transactions)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
