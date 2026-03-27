using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RequisitionManagement.API.Data;
using RequisitionManagement.API.DTOs;

namespace RequisitionManagement.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username && u.Password == dto.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(new
            {
                id = user.Id,
                username = user.Username,
                role = user.Role
            });
        }
    }
}