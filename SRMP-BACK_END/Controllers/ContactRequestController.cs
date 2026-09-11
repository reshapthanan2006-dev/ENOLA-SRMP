using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRMP.DTOs;
using SRMP.Interfaces;
using System.Security.Claims;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContactRequestController : ControllerBase
    {
        private readonly IContactRequestService _contactRequestService;

        public ContactRequestController(
            IContactRequestService contactRequestService)
        {
            _contactRequestService = contactRequestService;
        }

        // Employer sends contact request
        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CreateContactRequest(
            [FromBody] CreateContactRequestDto dto)
        {
            var employerId = GetUserId();

            if (employerId == null)
                return Unauthorized();

            try
            {
                var result =
                    await _contactRequestService
                        .CreateContactRequestAsync(
                            employerId.Value,
                            dto);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // Job Seeker views received requests
        [HttpGet("my")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> GetMyRequests()
        {
            var jobSeekerId = GetUserId();

            if (jobSeekerId == null)
                return Unauthorized();

            var result =
                await _contactRequestService
                    .GetMyRequestsAsync(
                        jobSeekerId.Value);

            return Ok(result);
        }

        // Employer views sent requests
        [HttpGet("sent")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetSentRequests()
        {
            var employerId = GetUserId();

            if (employerId == null)
                return Unauthorized();

            var result =
                await _contactRequestService
                    .GetSentRequestsAsync(
                        employerId.Value);

            return Ok(result);
        }

        // Job Seeker accepts or declines request
        [HttpPut("{contactRequestId}/respond")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> RespondToRequest(
            int contactRequestId,
            [FromQuery] string status)
        {
            var jobSeekerId = GetUserId();

            if (jobSeekerId == null)
                return Unauthorized();

            try
            {
                var result =
                    await _contactRequestService
                        .RespondToRequestAsync(
                            jobSeekerId.Value,
                            contactRequestId,
                            status);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message =
                            "Contact request not found or access denied."
                    });
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // Get logged-in User ID from JWT
        private int? GetUserId()
        {
            var userIdClaim =
                User.FindFirst(
                    ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return null;

            if (!int.TryParse(
                    userIdClaim.Value,
                    out var userId))
            {
                return null;
            }

            return userId;
        }
    }
}