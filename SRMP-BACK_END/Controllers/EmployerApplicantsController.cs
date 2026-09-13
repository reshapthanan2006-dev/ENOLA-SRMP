using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRMP.Interfaces;
using SRMP.Interfaces.Services;
using System.Security.Claims;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/employer/applicants")]
    [Authorize(Roles = "Employer")]
    public class EmployerApplicantsController : ControllerBase
    {
        private readonly IJobVacancyService _jobVacancyService;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobSeekerProfileRepository _profileRepository;
        private readonly IJobSeekerCvService _cvService;

        public EmployerApplicantsController(
            IJobVacancyService jobVacancyService,
            IApplicationRepository applicationRepository,
            IJobSeekerProfileRepository profileRepository,
            IJobSeekerCvService cvService)
        {
            _jobVacancyService = jobVacancyService;
            _applicationRepository = applicationRepository;
            _profileRepository = profileRepository;
            _cvService = cvService;
        }

        // =====================================================
        // APPLICANT PROFILE
        // =====================================================

        [HttpGet(
            "vacancy/{jobVacancyId}/jobseeker/{jobSeekerId}/profile"
        )]
        public async Task<IActionResult> GetApplicantProfile(
            int jobVacancyId,
            int jobSeekerId)
        {
            var employerId = GetUserId();

            if (employerId == null)
            {
                return Unauthorized();
            }

            var accessError =
                await ValidateApplicantAccess(
                    employerId.Value,
                    jobVacancyId,
                    jobSeekerId
                );

            if (accessError != null)
            {
                return accessError;
            }

            var profile =
                await _profileRepository
                    .GetByUserIdAsync(jobSeekerId);

            if (profile == null)
            {
                return NotFound(new
                {
                    message =
                        "Job seeker profile not found."
                });
            }

            return Ok(new
            {
                id = profile.Id,
                userId = profile.UserId,
                skills = profile.Skills,
                experienceYears =
                    profile.ExperienceYears,
                education = profile.Education,
                location = profile.Location
            });
        }

        // =====================================================
        // CV METADATA
        // =====================================================

        [HttpGet(
            "vacancy/{jobVacancyId}/jobseeker/{jobSeekerId}/cv"
        )]
        public async Task<IActionResult> GetApplicantCv(
            int jobVacancyId,
            int jobSeekerId)
        {
            var employerId = GetUserId();

            if (employerId == null)
            {
                return Unauthorized();
            }

            var accessError =
                await ValidateApplicantAccess(
                    employerId.Value,
                    jobVacancyId,
                    jobSeekerId
                );

            if (accessError != null)
            {
                return accessError;
            }

            var cv =
                await _cvService
                    .GetCvAsync(jobSeekerId);

            if (cv == null)
            {
                return NotFound(new
                {
                    message =
                        "Applicant has not uploaded a CV."
                });
            }

            return Ok(cv);
        }

        // =====================================================
        // CV DOWNLOAD / VIEW
        // =====================================================

        [HttpGet(
            "vacancy/{jobVacancyId}/jobseeker/{jobSeekerId}/cv/download"
        )]
        public async Task<IActionResult> DownloadApplicantCv(
            int jobVacancyId,
            int jobSeekerId)
        {
            var employerId = GetUserId();

            if (employerId == null)
            {
                return Unauthorized();
            }

            var accessError =
                await ValidateApplicantAccess(
                    employerId.Value,
                    jobVacancyId,
                    jobSeekerId
                );

            if (accessError != null)
            {
                return accessError;
            }

            try
            {
                var result =
                    await _cvService
                        .DownloadCvAsync(jobSeekerId);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message =
                            "Applicant has not uploaded a CV."
                    });
                }

                return File(
                    result.Value.FileBytes,
                    result.Value.ContentType,
                    result.Value.FileName
                );
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // ACCESS VALIDATION
        // =====================================================

        private async Task<IActionResult?>
            ValidateApplicantAccess(
                int employerId,
                int jobVacancyId,
                int jobSeekerId)
        {
            var vacancy =
                await _jobVacancyService
                    .GetByIdAsync(jobVacancyId);

            if (vacancy == null)
            {
                return NotFound(new
                {
                    message = "Vacancy not found."
                });
            }

            if (vacancy.EmployerId != employerId)
            {
                return Forbid();
            }

            var application =
                await _applicationRepository
                    .GetByJobSeekerAndVacancyAsync(
                        jobSeekerId,
                        jobVacancyId
                    );

            if (application == null)
            {
                return NotFound(new
                {
                    message =
                        "Application not found for this candidate."
                });
            }

            return null;
        }

        private int? GetUserId()
        {
            var userIdClaim =
                User.FindFirst(
                    ClaimTypes.NameIdentifier
                );

            if (userIdClaim == null)
            {
                return null;
            }

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