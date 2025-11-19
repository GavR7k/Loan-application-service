using LoanService.Data;
using LoanService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : Controller
    {

        private readonly LoanDbContext _db;
        public LoansController(LoanDbContext db)
        {
            _db = db;
        }

        [HttpGet] //GET/api/loans
        public async Task<ActionResult<List<LoanEntity>>> GetLoans()
        {
            var loans = await _db.LoanEntities.ToListAsync();
            return loans;
        }

        [HttpPost] //POST/api/loans
        public async Task<ActionResult<LoanEntity>> CreateLoan(LoanEntity loanEntity)
        {
            if (loanEntity.Amount <= 0)
                return BadRequest("Cумма займа должна быть больше 0");
            if (loanEntity.TermValue <= 0)
                return BadRequest("Срок займа должен быть больше 0");
            if (loanEntity.InterestValue <= 0)
                return BadRequest("Процентная ставка должна быть больше 0");

            loanEntity.Status = LoanStatus.Published;
            loanEntity.CreatedAt = DateTime.UtcNow;
            loanEntity.ModifiedAt = DateTime.UtcNow;

            await _db.LoanEntities.AddAsync(loanEntity);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateLoan), new { id = loanEntity.Id }, loanEntity);
        }
    }
}
