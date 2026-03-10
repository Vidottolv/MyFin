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
                AccountNumber = a.AccountNumber,
                InitialBalance = a.InitialBalance,
                CurrentBalance = a.CurrentBalance,
                TotalTransactions = _context.Transactions.Count(t => t.AccountId == a.AccountId),
                UserId = a.UserId,
                User = a.User
            });
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AccountDTO>> GetAccountById(Guid id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return NotFound();

            var request = new AccountDTO
            {
                AccountId = account.AccountId,
                Name = account.Name,
                Type = account.Type,
                AccountNumber = account.AccountNumber,
                InitialBalance = account.InitialBalance,
                CurrentBalance = account.CurrentBalance
            };

            return Ok(request);
        }

        [HttpGet("{accNumber}")]
        public async Task<ActionResult<AccountDTO>> GetAccountByNumber(string accNumber)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accNumber);
            if (account == null) return NotFound();

            var request = new AccountDTO
            {
                AccountId = account.AccountId,
                Name = account.Name,
                Type = account.Type,
                AccountNumber = account.AccountNumber,
                InitialBalance = account.InitialBalance,
                CurrentBalance = account.CurrentBalance,
                UserId = account.UserId
            };

            return Ok(request);
        }

        [HttpPost]
        public async Task<ActionResult<AccountDTO>> CreateAccount(CreateAccountDTO request)
        {
            var account = new TBLAccount
            {
                AccountId = Guid.NewGuid(),
                Name = request.Name,
                Type = request.Type,
                AccountNumber = request.AccountNumber,
                InitialBalance = request.InitialBalance,
                CurrentBalance = request.InitialBalance,
                UserId = request.UserId
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var result = new AccountDTO
            {
                AccountId = account.AccountId,
                Name = account.Name,
                Type = account.Type,
                AccountNumber = request.AccountNumber,
                InitialBalance = account.InitialBalance,
                CurrentBalance = account.CurrentBalance,
                UserId = account.UserId
            };

            return CreatedAtAction(nameof(GetAccountById), new { id = account.AccountId }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAccount(Guid id, UpdateAccountDTO dto)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Name)) account.Name = dto.Name;
            if (!string.IsNullOrWhiteSpace(dto.Type)) account.Type = dto.Type;
            if (dto.CurrentBalance >= 0) account.CurrentBalance = dto.CurrentBalance;

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
