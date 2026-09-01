using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRMP.DTOs;
using SRMP.Interfaces;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET: /api/admin/dashboard
        [HttpGet("dashboard")]
        public async Task<ActionResult<AdminDashboardDto>> GetDashboard()
        {
            var dashboard = await _adminService.GetDashboardAsync();

            return Ok(dashboard);
        }

        // GET: /api/admin/users
        [HttpGet("users")]
        public async Task<ActionResult<List<AdminUserDto>>> GetUsers()
        {
            var users = await _adminService.GetUsersAsync();

            return Ok(users);
        }

        // PUT: /api/admin/users/5/status
        [HttpPut("users/{userId}/status")]
        public async Task<ActionResult<AdminUserDto>> UpdateUserStatus(
            int userId,
            UpdateUserStatusDto updateDto)
        {
            var user = await _adminService.UpdateUserStatusAsync(
                userId,
                updateDto
            );

            return Ok(user);
        }
    }
}