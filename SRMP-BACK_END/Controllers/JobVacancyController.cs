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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vacancy = await _service.GetByIdAsync(id);

            if (vacancy == null)
                return NotFound("Vacancy not found.");

            return Ok(vacancy);
        }

        [HttpGet("employer/{employerId}")]
        public async Task<IActionResult> GetByEmployerId(int employerId)
        {
            var vacancies = await _service.GetByEmployerIdAsync(employerId);

            return Ok(vacancies);
        }

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

        [HttpPost]
        public async Task<IActionResult> Create(JobVacancy vacancy)
        {
            await _service.CreateAsync(vacancy);

            return CreatedAtAction(
                nameof(GetById),
                new { id = vacancy.JobVacancyId },
                vacancy);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            JobVacancy vacancy)
        {
            var existingVacancy = await _service.GetByIdAsync(id);

            if (existingVacancy == null)
                return NotFound("Vacancy not found.");

            if (existingVacancy.EmployerId != vacancy.EmployerId)
                return Unauthorized(
                    "You are not allowed to update this vacancy.");

            vacancy.JobVacancyId = id;

            await _service.UpdateAsync(vacancy);

            return Ok(vacancy);
        }

        [HttpPut("{id}/close")]
        public async Task<IActionResult> Close(
            int id,
            [FromQuery] int employerId)
        {
            try
            {
                await _service.CloseAsync(id, employerId);

                return Ok("Vacancy closed successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}