using Microsoft.EntityFrameworkCore;
using RequisitionManagement.API.Data;
using RequisitionManagement.API.Models;

namespace RequisitionManagement.API.Repositories
{
    public class RequisitionRepository
    {
        private readonly AppDbContext _context;

        public RequisitionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Requisition> CreateAsync(Requisition requisition)
        {
            _context.Requisitions.Add(requisition);
            await _context.SaveChangesAsync();
            return requisition;
        }

        public async Task<List<Requisition>> GetByCreatorAsync(int createdBy)
        {
            return await _context.Requisitions
                .Include(r => r.Creator)
                .Where(r => r.CreatedBy == createdBy)
                .ToListAsync();
        }

        public async Task<Requisition> GetByIdAsync(int id)
        {
            return await _context.Requisitions
                .Include(r => r.Creator)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Requisition>> GetByStatusAsync(string status)
        {
            return await _context.Requisitions
                .Include(r => r.Creator)
                .Where(r => r.Status == status)
                .ToListAsync();
        }

        public async Task UpdateAsync(Requisition requisition)
        {
            _context.Requisitions.Update(requisition);
            await _context.SaveChangesAsync();
        }

        public async Task AddApprovalAsync(Approval approval)
        {
            _context.Approvals.Add(approval);
            await _context.SaveChangesAsync();
        }
    }
}