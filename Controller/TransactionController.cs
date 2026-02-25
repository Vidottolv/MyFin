using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFin.Application.DTOs;
using MyFin.Domain;
using MyFin.Domain.Entities;
using MyFin.Persistence;

namespace MyFin.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TransactionController(AppDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetTransactions([FromQuery] Guid userId)
        {
            var Transaction = await _context.Transactions
                .Where(x => x.UserId == userId)
                .ToListAsync();

            var result = Transaction.Select(a => new TransactionDTO
            {
                TransactionId = a.TransactionId,
                Type = a.Type,
                UserId = a.UserId,
                Description = a.Description,
                Amount = a.Amount
            });
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TransactionDTO>> GetTransaction(Guid id)
        {
            var Transaction = await _context.Transactions.FindAsync(id);
            if (Transaction == null) return NotFound();

            var dto = new TransactionDTO
            {
                TransactionId = Transaction.TransactionId,
                Type = Transaction.Type,
                Description = Transaction.Description,
                Amount = Transaction.Amount
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDTO>> CreateTransaction(CreateTransactionDTO dto)
        {
            var account = await _context.Accounts.FindAsync(dto.AccountId);
            if (account == null) return NotFound("Conta não encontrada.");

            var Transaction = new TBLTransaction
            {
                TransactionId = Guid.NewGuid(),
                Type = dto.Type,
                UserId = dto.UserId,
                Description = dto.Description,
                Amount =  dto.Type == TransactionType.Income ? dto.Amount : -dto.Amount,
                CategoryId = dto.CategoryId,
                AccountId = dto.AccountId
            };
            _context.Transactions.Add(Transaction);
            await _context.SaveChangesAsync();

            account.CurrentBalance += Transaction.Amount;
            await _context.SaveChangesAsync();

            var result = new TransactionDTO
            {
                TransactionId = Transaction.TransactionId,
                Type = Transaction.Type,
                UserId = Transaction.UserId
            };

            return CreatedAtAction(nameof(GetTransaction), new { id = Transaction.TransactionId }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTransaction(Guid id, UpdateTransactionDTO dto)
        {
            var Transaction = await _context.Transactions.FindAsync(id);
            if (Transaction == null) return NotFound();



            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {
            var Transaction = await _context.Transactions.FindAsync(id);
            if (Transaction == null) return NotFound();

            _context.Transactions.Remove(Transaction);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
