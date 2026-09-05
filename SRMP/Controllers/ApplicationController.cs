using Microsoft.AspNetCore.Mvc;
using SRMP.DTOs;
using SRMP.Interfaces;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        // Job Seeker applies for a vacancy
        [HttpPost]
        public async Task<IActionResult> CreateApplication(
            [FromQuery] int jobSeekerId,
            [FromBody] CreateApplicationDto dto)
        {
            try
            {
                var result = await _applicationService
                    .CreateApplicationAsync(jobSeekerId, dto);

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

        // Job Seeker views own applications
        [HttpGet("my")]
        public async Task<IActionResult> GetMyApplications(
            [FromQuery] int jobSeekerId)
        {
            var result = await _applicationService
                .GetMyApplicationsAsync(jobSeekerId);

            return Ok(result);
        }

        // Employer views applications for a vacancy
        [HttpGet("vacancy/{jobVacancyId}")]
        public async Task<IActionResult> GetApplicationsByVacancy(
            int jobVacancyId,
            [FromQuery] int employerId)
        {
            try
            {
                var result = await _applicationService
                    .GetApplicationsByVacancyAsync(
                        employerId,
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
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }

        // Employer updates application status
        [HttpPut("{applicationId}/status")]
        public async Task<IActionResult> UpdateApplicationStatus(
            int applicationId,
            [FromQuery] int employerId,
            [FromBody] UpdateApplicationStatusDto dto)
        {
            var result = await _applicationService
                .UpdateApplicationStatusAsync(
                    employerId,
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
    }
}