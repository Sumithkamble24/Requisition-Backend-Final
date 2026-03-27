using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RequisitionManagement.API.Data;

namespace RequisitionManagement.API.Controllers
{
    [ApiController]
    [Route("api/recruiter")]
    public class RecruiterController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecruiterController(AppDbContext context)
        {
            _context = context;
        }

        // Recruiter views all L3Approved requisitions
        [HttpGet("requisitions")]
        public async Task<IActionResult> GetApprovedRequisitions()
        {
            var requisitions = await _context.Requisitions
                .Where(r => r.Status == "L3Approved")
                .ToListAsync();

            return Ok(requisitions);
        }

        
        // Recruiter views all closed requisitions
        [HttpGet("requisitions/closed")]
        public async Task<IActionResult> GetClosedRequisitions()
        {
            var requisitions = await _context.Requisitions
                .Include(r => r.Creator)
                .Where(r => r.Status == "Closed")
                .ToListAsync();

            return Ok(new { totalClosed = requisitions.Count, requisitions = requisitions });
        }

        // Recruiter closes a requisition
        [HttpPost("requisitions/{id}/close")]
        public async Task<IActionResult> CloseRequisition(int id)
        {
            var requisition = await _context.Requisitions.FindAsync(id);
            if (requisition == null) return NotFound();

            if (requisition.Status != "L3Approved")
                return BadRequest(new { message = "Only L3Approved requisitions can be closed" });

            requisition.Status = "Closed";
            _context.Requisitions.Update(requisition);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Requisition closed successfully", status = requisition.Status });
        }
    }
}