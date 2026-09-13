using Microsoft.AspNetCore.Mvc;
using SRMP.DTOs;
using SRMP.DTOs.Auth;
using SRMP.Interfaces;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService)
        {
            _authService = authService;
        }


        // =====================================================
        // REGISTER
        // =====================================================

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterDto dto)
        {
            try
            {
                var result =
                    await _authService
                        .RegisterAsync(dto);

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // LOGIN
        // =====================================================

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto dto)
        {
            try
            {
                var result =
                    await _authService
                        .LoginAsync(dto);

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // FORGOT PASSWORD
        // =====================================================

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordDto dto)
        {
            await _authService
                .ForgotPasswordAsync(dto);

            /*
             * Always return the same response.
             *
             * This prevents attackers from discovering
             * which email addresses are registered.
             */
            return Ok(new
            {
                message =
                    "If an account exists for this email, password reset instructions will be sent."
            });
        }


        // =====================================================
        // RESET PASSWORD
        // =====================================================

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordDto dto)
        {
            try
            {
                await _authService
                    .ResetPasswordAsync(dto);

                return Ok(new
                {
                    message =
                        "Your password has been reset successfully."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}