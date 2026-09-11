using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRMP.DTOs.JobSeeker;
using SRMP.Interfaces;
using SRMP.Interfaces.Services;
using SRMP.Models;
using System.Security.Claims;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobVacancyController : ControllerBase
    {
        private readonly IJobVacancyService _service;
        private readonly IJobSeekerProfileRepository _profileRepository;
        private readonly IMatchingService _matchingService;

        public JobVacancyController(
            IJobVacancyService service,
            IJobSeekerProfileRepository profileRepository,
            IMatchingService matchingService)
        {
            _service = service;
            _profileRepository = profileRepository;
            _matchingService = matchingService;
        }

        // Get vacancy by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vacancy = await _service.GetByIdAsync(id);

            if (vacancy == null)
                return NotFound("Vacancy not found.");

            return Ok(vacancy);
        }

        // Employer views own vacancies
        [HttpGet("my")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetMyVacancies()
        {
            var employerId = GetUserId();

            if (employerId == null)
                return Unauthorized();

            var vacancies = await _service
                .GetByEmployerIdAsync(employerId.Value);

            return Ok(vacancies);
        }

        // Job seeker searches open vacancies
        [HttpGet("search")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> Search(
            [FromQuery] string? keyword,
            [FromQuery] string? location,
            [FromQuery] int? minExperience)
        {
            var vacancies = await _service.SearchOpenVacanciesAsync(
                keyword,
                location,
                minExperience);

            return Ok(vacancies);
        }

        // Job seeker views vacancy with match details
        [HttpGet("{id}/jobseeker-detail")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> GetJobSeekerJobDetail(int id)
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null ||
                !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var profile =
                await _profileRepository.GetByUserIdAsync(userId);

            if (profile == null)
            {
                return NotFound(new
                {
                    message = "Job seeker profile not found."
                });
            }

            var vacancy = await _service.GetByIdAsync(id);

            if (vacancy == null)
            {
                return NotFound(new
                {
                    message = "Vacancy not found."
                });
            }

            var requiredSkills = vacancy.RequiredSkills
                .Split(
                    ',',
                    StringSplitOptions.RemoveEmptyEntries |
                    StringSplitOptions.TrimEntries)
                .ToList();

            var matchingVacancy = new Vacancy
            {
                Id = vacancy.JobVacancyId,
                RequiredSkills = requiredSkills,
                RequiredExperienceYears =
                    vacancy.RequiredExperience
            };

            var matchResult = _matchingService.CalculateMatch(
                profile,
                matchingVacancy);

            var response = new JobSeekerJobDetailDto
            {
                JobVacancyId = vacancy.JobVacancyId,
                Title = vacancy.Title,
                Description = vacancy.Description,
                RequiredSkills = vacancy.RequiredSkills,
                RequiredExperience = vacancy.RequiredExperience,
                Location = vacancy.Location,
                IsOpen = vacancy.IsOpen,
                MatchScore = matchResult.MatchScore,
                MissingSkills = matchResult.MissingSkills
            };

            return Ok(response);
        }

        // Employer creates own vacancy
        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Create(
            JobVacancy vacancy)
        {
            var employerId = GetUserId();

            if (employerId == null)
                return Unauthorized();

            // EmployerId comes from JWT
            vacancy.EmployerId = employerId.Value;

            await _service.CreateAsync(vacancy);

            return CreatedAtAction(
                nameof(GetById),
                new { id = vacancy.JobVacancyId },
                vacancy);
        }

        // Employer updates own vacancy
        [HttpPut("{id}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Update(
            int id,
            JobVacancy vacancy)
        {
            var employerId = GetUserId();

            if (employerId == null)
                return Unauthorized();

            var existingVacancy =
                await _service.GetByIdAsync(id);

            if (existingVacancy == null)
                return NotFound("Vacancy not found.");

            // Employer can update only own vacancy
            if (existingVacancy.EmployerId != employerId.Value)
                return Forbid();

            vacancy.JobVacancyId = id;

            // Never trust EmployerId from frontend
            vacancy.EmployerId = employerId.Value;

            await _service.UpdateAsync(vacancy);

            return Ok(vacancy);
        }

        // Employer closes own vacancy
        [HttpPut("{id}/close")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Close(int id)
        {
            var employerId = GetUserId();

            if (employerId == null)
                return Unauthorized();

            try
            {
                await _service.CloseAsync(
                    id,
                    employerId.Value);

                return Ok(new
                {
                    message = "Vacancy closed successfully."
                });
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

        // Get logged-in user id from JWT
        private int? GetUserId()
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

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