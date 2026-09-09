using Microsoft.AspNetCore.Mvc;
using SRMP.DTOs;
using SRMP.Interfaces;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactRequestController : ControllerBase
    {
        private readonly IContactRequestService _contactRequestService;

        public ContactRequestController(
            IContactRequestService contactRequestService)
        {
            _contactRequestService = contactRequestService;
        }

        // Employer sends a contact request
        [HttpPost]
        public async Task<IActionResult> CreateContactRequest(
            [FromQuery] int employerId,
            [FromBody] CreateContactRequestDto dto)
        {
            try
            {
                var result = await _contactRequestService
                    .CreateContactRequestAsync(employerId, dto);

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        // Job Seeker views received contact requests
        [HttpGet("my")]
        public async Task<IActionResult> GetMyRequests(
            [FromQuery] int jobSeekerId)
        {
            var result = await _contactRequestService
                .GetMyRequestsAsync(jobSeekerId);

            return Ok(result);
        }

        // Employer views sent contact requests
        [HttpGet("sent")]
        public async Task<IActionResult> GetSentRequests(
            [FromQuery] int employerId)
        {
            var result = await _contactRequestService
                .GetSentRequestsAsync(employerId);

            return Ok(result);
        }

        // Job Seeker accepts or declines a contact request
        [HttpPut("{contactRequestId}/respond")]
        public async Task<IActionResult> RespondToRequest(
            int contactRequestId,
            [FromQuery] int jobSeekerId,
            [FromQuery] string status)
        {
            try
            {
                var result = await _contactRequestService
                    .RespondToRequestAsync(
                        jobSeekerId,
                        contactRequestId,
                        status);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message = "Contact request not found or access denied."
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
    }
}