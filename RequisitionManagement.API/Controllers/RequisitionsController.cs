using Microsoft.AspNetCore.Mvc;
using RequisitionManagement.API.DTOs;
using RequisitionManagement.API.Services;

namespace RequisitionManagement.API.Controllers
{
    [ApiController]
    [Route("api/requisitions")]
    public class RequisitionsController : ControllerBase
    {
        private readonly RequisitionService _service;

        public RequisitionsController(RequisitionService service)
        {
            _service = service;
        }

        // CU Manager creates a requisition
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRequisitionDto dto, [FromQuery] int createdBy)
        {
            var result = await _service.CreateRequisitionAsync(dto, createdBy);
            return Ok(result);
        }

        // CU Manager views their own requisitions
        [HttpGet("my")]
        public async Task<IActionResult> GetMy([FromQuery] int createdBy)
        {
            var result = await _service.GetMyRequisitionsAsync(createdBy);
            return Ok(result);
        }

        // View single requisition by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetRequisitionByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}