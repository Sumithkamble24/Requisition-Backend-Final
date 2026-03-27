using RequisitionManagement.API.DTOs;
using RequisitionManagement.API.Models;
using RequisitionManagement.API.Repositories;

namespace RequisitionManagement.API.Services
{
    public class RequisitionService
    {
        private readonly RequisitionRepository _repository;

        public RequisitionService(RequisitionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Requisition> CreateRequisitionAsync(CreateRequisitionDto dto, int createdBy)
        {
            var requisition = new Requisition
            {
                Department = dto.Department,
                Skillset = dto.Skillset,
                ExperienceLevel = dto.ExperienceLevel,
                NumberOfPositions = dto.NumberOfPositions,
                CreatedBy = createdBy,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            return await _repository.CreateAsync(requisition);
        }

        public async Task<List<Requisition>> GetMyRequisitionsAsync(int createdBy)
        {
            return await _repository.GetByCreatorAsync(createdBy);
        }

        public async Task<Requisition> GetRequisitionByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}