using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRMP.Interfaces;
using SRMP.Models;
using System.Security.Claims;

namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployerCompanyController : ControllerBase
    {
        private readonly IEmployerCompanyService _service;

        public EmployerCompanyController(
            IEmployerCompanyService service)
        {
            _service = service;
        }

        // Logged-in Employer views own company
        [HttpGet("my")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetMyCompany()
        {
            var employerId = GetUserId();

            if (employerId == null)
                return Unauthorized();

            var company =
                await _service.GetCompanyByEmployerIdAsync(
                    employerId.Value);

            if (company == null)
            {
                return NotFound(new
                {
                    message = "Company profile not found."
                });
            }

            return Ok(company);
        }

        // Get company by company id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company =
                await _service.GetCompanyByIdAsync(id);

            if (company == null)
            {
                return NotFound(new
                {
                    message = "Company profile not found."
                });
            }

            return Ok(company);
        }

        // Employer creates own company profile
        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Create(
            EmployerCompany company)
        {
            var employerId = GetUserId();

            if (employerId == null)
                return Unauthorized();

            // EmployerId must come from JWT
            company.EmployerId = employerId.Value;

            try
            {
                await _service.CreateCompanyAsync(company);

                return CreatedAtAction(
                    nameof(GetById),
                    new
                    {
                        id = company.EmployerCompanyId
                    },
                    company);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        // Employer updates own company profile only
        [HttpPut("{id}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Update(
            int id,
            EmployerCompany company)
        {
            var employerId = GetUserId();

            if (employerId == null)
                return Unauthorized();

            var existingCompany =
                await _service.GetCompanyByIdAsync(id);

            if (existingCompany == null)
            {
                return NotFound(new
                {
                    message = "Company profile not found."
                });
            }

            // Employer can update only own company
            if (existingCompany.EmployerId != employerId.Value)
            {
                return Forbid();
            }

            // Update the already tracked entity
            existingCompany.CompanyName = company.CompanyName;
            existingCompany.Description = company.Description;

            // Do not take EmployerId from frontend
            existingCompany.EmployerId = employerId.Value;

            // Keep original CreatedAt
            // Update only UpdatedAt
            existingCompany.UpdatedAt = DateTime.UtcNow;

            await _service.UpdateCompanyAsync(existingCompany);

            return Ok(existingCompany);
        }

        // Get logged-in User ID from JWT
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