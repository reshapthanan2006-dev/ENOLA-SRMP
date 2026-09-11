using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRMP.Interfaces;
using System.Security.Claims;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/employer/applicants")]
    [Authorize(Roles = "Employer")]
    public class ApplicantProfileController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IJobSeekerProfileService _profileService;

        public ApplicantProfileController(
            IApplicationService applicationService,
            IJobSeekerProfileService profileService)
        {
            _applicationService = applicationService;
            _profileService = profileService;
        }

        [HttpGet("vacancy/{jobVacancyId}/jobseeker/{jobSeekerId}/profile")]
        public async Task<IActionResult> GetApplicantProfile(
            int jobVacancyId,
            int jobSeekerId)
        {
            var employerId = GetUserId();

            if (employerId == null)
                return Unauthorized();

            try
            {
                var applications =
                    await _applicationService.GetApplicationsByVacancyAsync(
                        employerId.Value,
                        jobVacancyId);

                var applicantExists =
                    applications.Any(application =>
                        application.JobSeekerId == jobSeekerId);

                if (!applicantExists)
                {
                    return NotFound(new
                    {
                        message = "Applicant not found for this vacancy."
                    });
                }

                var profile =
                    await _profileService.GetProfileAsync(jobSeekerId);

                if (profile == null)
                {
                    return NotFound(new
                    {
                        message = "Job seeker profile not found."
                    });
                }

                return Ok(profile);
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

        private int? GetUserId()
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return null;

            if (!int.TryParse(userIdClaim.Value, out var userId))
                return null;

            return userId;
        }
    }
}