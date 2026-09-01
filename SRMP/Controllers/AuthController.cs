using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRMP.DTOs;
using SRMP.Interfaces;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: /api/auth/register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(
            RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);

            return Ok(result);
        }

        // POST: /api/auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(
            LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);

            return Ok(result);
        }
    }
}