using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFin.Application.DTOs;
using MyFin.Persistence;

namespace MyFin.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context) { _context = context; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAccounts([FromQuery] Guid userId)
        {
            var accounts = await _context.Accounts
                .Where(x => x.UserId == userId)
                .ToListAsync();

            var result = accounts.Select(a => new AccountDTO
            {
                AccountId = a.AccountId,
                Name = a.Name,
                Type = a.Type,
                InitialBalance = a.InitialBalance,
                CurrentBalance = a.CurrentBalance,
                TotalTransactions = _context.Transactions.Count(t => t.AccountId == a.AccountId)
            });
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AccountDTO>> GetAccount(Guid id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return NotFound();

            var dto = new AccountDTO
            {
                AccountId = account.AccountId,
                Name = account.Name,
                Type = account.Type,
                InitialBalance = account.InitialBalance,
                CurrentBalance = account.CurrentBalance
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<AccountDTO>> CreateAccount(AccountDTO dto)
        {
            var account = new TBLAccount
            {
                AccountId = Guid.NewGuid(),
                Name = dto.Name,
                Type = dto.Type,
                InitialBalance = dto.InitialBalance,
                CurrentBalance = dto.InitialBalance,
                UserId = dto.UserId
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var result = new AccountDTO
            {
                AccountId = account.AccountId,
                Name = account.Name,
                Type = account.Type,
                InitialBalance = account.InitialBalance,
                CurrentBalance = account.CurrentBalance
            };

            return CreatedAtAction(nameof(GetAccount), new { id = account.AccountId }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAccount(Guid id, AccountDTO dto)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return NotFound();

            if (string.IsNullOrWhiteSpace(dto.Name))
                account.Name = dto.Name;
            if (dto.CurrentBalance >= 0)
                account.CurrentBalance = dto.CurrentBalance;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return NotFound();

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
