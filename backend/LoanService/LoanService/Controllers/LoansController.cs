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
        public async Task<ActionResult<List<LoanEntity>>> GetLoans(
            [FromQuery] LoanStatus? status,
            [FromQuery] decimal? minAmount,
            [FromQuery] decimal? maxAmount,
            [FromQuery] int? minTerm,
            [FromQuery] int? maxTerm
            )
        {
            // позволяет динамически добавлять условия

            var query = _db.LoanEntities.AsQueryable();

            if (status.HasValue)
                query = query.Where(l => l.Status == status.Value);

            if (minAmount.HasValue)
                query = query.Where(l => l.Amount >= minAmount.Value);

            if (maxAmount.HasValue)
                query = query.Where(l => l.Amount <= maxAmount.Value);

            if (minTerm.HasValue)
                query = query.Where(l => l.TermValue >= minTerm.Value);

            if (maxTerm.HasValue)
                query = query.Where(l => l.TermValue <= maxTerm.Value);

            query = query.OrderByDescending(l => l.Id);

            var loans = await query.ToListAsync();
            return Ok(loans);

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


        [HttpPatch("{id}/change-status")] //PATCH /api/loans
        public async Task<ActionResult<LoanEntity>> ChangeStatus(int id)
        {
            var loan = await _db.LoanEntities.FindAsync(id);
            if (loan == null)
                return NotFound();

            loan.Status = loan.Status == LoanStatus.Published ? LoanStatus.Unpublished : LoanStatus.Published;

            loan.ModifiedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok(loan);

        }

    }
}
