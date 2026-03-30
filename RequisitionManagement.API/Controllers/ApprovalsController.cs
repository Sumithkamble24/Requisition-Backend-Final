using Microsoft.AspNetCore.Mvc;
using RequisitionManagement.API.Data;
using RequisitionManagement.API.DTOs;
using RequisitionManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace RequisitionManagement.API.Controllers
{
    [ApiController]
    [Route("api/approvals")]
    public class ApprovalsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApprovalsController(AppDbContext context)
        {
            _context = context;
        }

        // BU Manager sees Pending, L3 Manager sees BUApproved
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending([FromQuery] string role)
        {
            string status = role == "BU_MANAGER" ? "Pending" : "BUApproved";

            var requisitions = await _context.Requisitions
                .Include(r => r.Creator)
                .Where(r => r.Status == status)
                .ToListAsync();

            return Ok(requisitions);
        }

        // Approve
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id, [FromBody] ApprovalDto dto)
        {
            var requisition = await _context.Requisitions.FindAsync(id);
            if (requisition == null) return NotFound();

            var approver = await _context.Users.FindAsync(dto.ApproverId);
            if (approver == null) return NotFound(new { message = "Approver not found" });

            if (approver.Role == "BU_MANAGER" && requisition.Status == "Pending")
                requisition.Status = "BUApproved";
            else if (approver.Role == "L3_MANAGER" && requisition.Status == "BUApproved")
                requisition.Status = "L3Approved";
            else
                return BadRequest(new { message = "Invalid approval action" });

            _context.Requisitions.Update(requisition);

            var approval = new Approval
            {
                RequisitionId = id,
                ApproverId = dto.ApproverId,
                ApprovalLevel = approver.Role == "BU_MANAGER" ? "BU" : "L3",
                Status = "Approved",
                Comments = dto.Comments,
                ActionDate = DateTime.Now
            };

            _context.Approvals.Add(approval);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Approved successfully", status = requisition.Status });
        }

        // Reject
        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(int id, [FromBody] ApprovalDto dto)
        {
            var requisition = await _context.Requisitions.FindAsync(id);
            if (requisition == null) return NotFound();

            var approver = await _context.Users.FindAsync(dto.ApproverId);
            if (approver == null) return NotFound(new { message = "Approver not found" });

            if (requisition.Status != "Pending" && requisition.Status != "BUApproved")
                return BadRequest(new { message = "Cannot reject at this stage" });

            requisition.Status = "Rejected";
            _context.Requisitions.Update(requisition);

            var approval = new Approval
            {
                RequisitionId = id,
                ApproverId = dto.ApproverId,
                ApprovalLevel = approver.Role == "BU_MANAGER" ? "BU" : "L3",
                Status = "Rejected",
                Comments = dto.Comments,
                ActionDate = DateTime.Now
            };

            _context.Approvals.Add(approval);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Rejected successfully", status = requisition.Status });
        }

        // ✅ NEW: Get approval history for a requisition (ADDED ONLY)
        [HttpGet("{requisitionId}/history")]
        public async Task<IActionResult> GetHistory(int requisitionId)
        {
            var history = await _context.Approvals
                .Include(a => a.Approver)
                .Where(a => a.RequisitionId == requisitionId)
                .OrderBy(a => a.ActionDate)
                .Select(a => new
                {
                    id = a.Id,
                    requisitionId = a.RequisitionId,
                    approverId = a.ApproverId,
                    approvalLevel = a.ApprovalLevel,
                    status = a.Status,
                    comments = a.Comments,
                    actionDate = a.ActionDate,

                    // null safety
                    approver = a.Approver == null ? null : new
                    {
                        id = a.Approver.Id,
                        username = a.Approver.Username,
                        role = a.Approver.Role
                    }
                })
                .ToListAsync();

            if (history == null || history.Count == 0)
            {
                return Ok(new List<object>());
            }

            return Ok(history);
        }
    }
}