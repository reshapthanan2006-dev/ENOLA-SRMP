using Microsoft.AspNetCore.Mvc;
using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployerCompanyController : ControllerBase
    {
        private readonly IEmployerCompanyService _service;

        public EmployerCompanyController(IEmployerCompanyService service)
        {
            _service = service;
        }

        [HttpGet("employer/{employerId}")]
        public async Task<IActionResult> GetByEmployerId(int employerId)
        {
            var company = await _service.GetCompanyByEmployerIdAsync(employerId);

            if (company == null)
                return NotFound("Company profile not found.");

            return Ok(company);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _service.GetCompanyByIdAsync(id);

            if (company == null)
                return NotFound("Company profile not found.");

            return Ok(company);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployerCompany company)
        {
            try
            {
                await _service.CreateCompanyAsync(company);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = company.EmployerCompanyId },
                    company);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            EmployerCompany company)
        {
            var existingCompany =
                await _service.GetCompanyByIdAsync(id);

            if (existingCompany == null)
                return NotFound("Company profile not found.");

            // Ownership check
            if (existingCompany.EmployerId != company.EmployerId)
            {
                return Unauthorized(
                    "You are not allowed to update this company profile.");
            }

            company.EmployerCompanyId = id;

            await _service.UpdateCompanyAsync(company);

            return Ok(company);
        }
    }
}