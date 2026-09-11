using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRMP.DTOs.JobSeeker;
using SRMP.Interfaces;
using System.Security.Claims;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/jobseeker/profile")]
    [Authorize(Roles = "JobSeeker")]
    public class JobSeekerProfileController : ControllerBase
    {
        private readonly IJobSeekerProfileService _service;

        public JobSeekerProfileController(
            IJobSeekerProfileService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var profile = await _service.GetProfileAsync(userId.Value);

            if (profile == null)
                return NotFound(new
                {
                    message = "Job seeker profile not found."
                });

            return Ok(profile);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile(
            [FromBody] CreateJobSeekerProfileDto dto)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            try
            {
                var profile = await _service.CreateProfileAsync(
                    userId.Value,
                    dto);

                return CreatedAtAction(
                    nameof(GetProfile),
                    null,
                    profile);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] CreateJobSeekerProfileDto dto)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            try
            {
                var profile = await _service.UpdateProfileAsync(
                    userId.Value,
                    dto);

                return Ok(profile);
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