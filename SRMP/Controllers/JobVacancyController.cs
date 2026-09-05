using Microsoft.AspNetCore.Mvc;
using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobVacancyController : ControllerBase
    {
        private readonly IJobVacancyService _service;

        public JobVacancyController(IJobVacancyService service)
        {
            _service = service;
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