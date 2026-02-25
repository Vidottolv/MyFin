using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFin.Application.DTOs;
using MyFin.Domain.Entities;
using MyFin.Persistence;

namespace MyFin.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriesDTO>>> GetCategorys([FromQuery] Guid userId)
        {
            var Categories = await _context.Categories
                .Where(x => x.UserId == userId)
                .ToListAsync();

            var result = Categories.Select(a => new CategoriesDTO
            {
                CategoryId = a.CategoryId,
                Name = a.Name,
                Type = a.Type,
                UserId = a.UserId
            });
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoriesDTO>> GetCategory(Guid id)
        {
            var Category = await _context.Categories.FindAsync(id);
            if (Category == null) return NotFound();

            var dto = new CategoriesDTO
            {
                CategoryId = Category.CategoryId,
                Name = Category.Name,
                Type = Category.Type
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriesDTO>> CreateCategory(CreateCategoryDTO dto)
        {
            var Category = new TBLCategory
            {
                CategoryId = Guid.NewGuid(),
                Name = dto.Name,
                Type = dto.Type,
                UserId = dto.UserId
            };

            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();

            var result = new CategoriesDTO
            {
                CategoryId = Category.CategoryId,
                Name = Category.Name,
                Type = Category.Type,
                UserId = Category.UserId
            };

            return CreatedAtAction(nameof(GetCategory), new { id = Category.CategoryId }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryDTO dto)
        {
            var Category = await _context.Categories.FindAsync(id);
            if (Category == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Name)) Category.Name = dto.Name;
            if (!string.IsNullOrWhiteSpace(dto.Type)) Category.Type = dto.Type;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var Category = await _context.Categories.FindAsync(id);
            if (Category == null) return NotFound();

            _context.Categories.Remove(Category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
