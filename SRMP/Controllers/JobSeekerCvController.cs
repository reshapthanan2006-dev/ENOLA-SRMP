using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRMP.Interfaces.Services;
using System.Security.Claims;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/jobseeker/cv")]
    [Authorize(Roles = "JobSeeker")]
    public class JobSeekerCvController : ControllerBase
    {
        private readonly IJobSeekerCvService _service;

        public JobSeekerCvController(
            IJobSeekerCvService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCv()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var cv = await _service.GetCvAsync(userId.Value);

            if (cv == null)
            {
                return NotFound(new
                {
                    message = "CV not found."
                });
            }

            return Ok(cv);
        }

        [HttpPost("upload")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> UploadCv(
            IFormFile file)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            try
            {
                var cv = await _service.UploadCvAsync(
                    userId.Value,
                    file);

                return Ok(cv);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCv()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            try
            {
                await _service.DeleteCvAsync(userId.Value);

                return Ok(new
                {
                    message = "CV deleted successfully."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }

        private int? GetUserId()
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return null;

            if (!int.TryParse(
                    userIdClaim.Value,
                    out var userId))
                return null;

            return userId;
        }
    }
}