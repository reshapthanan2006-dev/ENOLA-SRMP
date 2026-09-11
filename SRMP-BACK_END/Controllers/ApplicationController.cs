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
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(
            IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        // Job Seeker applies for vacancy
        [HttpPost]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> CreateApplication(
            [FromBody] CreateApplicationDto dto)
        {
            var jobSeekerId = GetUserId();

            if (jobSeekerId == null)
                return Unauthorized();

            try
            {
                var result =
                    await _applicationService
                        .CreateApplicationAsync(
                            jobSeekerId.Value,
                            dto);

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
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

        // Job Seeker views own applications
        [HttpGet("my")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> GetMyApplications()
        {
            var jobSeekerId = GetUserId();

            if (jobSeekerId == null)
                return Unauthorized();

            var result =
                await _applicationService
                    .GetMyApplicationsAsync(
                        jobSeekerId.Value);

            return Ok(result);
        }

        // Employer views applications for vacancy
        [HttpGet("vacancy/{jobVacancyId}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult>
            GetApplicationsByVacancy(int jobVacancyId)
        {
            var employerId = GetUserId();

            if (employerId == null)
                return Unauthorized();

            try
            {
                var result =
                    await _applicationService
                        .GetApplicationsByVacancyAsync(
                            employerId.Value,
                            jobVacancyId);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // Employer views ranked applicants
        [HttpGet("vacancy/{jobVacancyId}/ranked")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult>
            GetRankedApplicants(int jobVacancyId)
        {
            var employerId = GetUserId();

            if (employerId == null)
                return Unauthorized();

            try
            {
                var result =
                    await _applicationService
                        .GetRankedApplicantsAsync(
                            employerId.Value,
                            jobVacancyId);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // Employer updates application status
        [HttpPut("{applicationId}/status")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult>
            UpdateApplicationStatus(
                int applicationId,
                [FromBody] UpdateApplicationStatusDto dto)
        {
            var employerId = GetUserId();

            if (employerId == null)
                return Unauthorized();

            try
            {
                var result =
                    await _applicationService
                        .UpdateApplicationStatusAsync(
                            employerId.Value,
                            applicationId,
                            dto);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message = "Application not found."
                    });
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
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