using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Services
{
    public class EmployerCompanyService : IEmployerCompanyService
    {
        private readonly IEmployerCompanyRepository _repository;

        public EmployerCompanyService(IEmployerCompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task<EmployerCompany?> GetCompanyByEmployerIdAsync(int employerId)
        {
            return await _repository.GetByEmployerIdAsync(employerId);
        }

        public async Task<EmployerCompany?> GetCompanyByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateCompanyAsync(EmployerCompany company)
        {
            var existingCompany =
                await _repository.GetByEmployerIdAsync(company.EmployerId);

            if (existingCompany != null)
            {
                throw new InvalidOperationException(
                    "Company profile already exists for this employer.");
            }

            await _repository.AddAsync(company);
        }

        public async Task UpdateCompanyAsync(EmployerCompany company)
        {
            await _repository.UpdateAsync(company);
        }
    }
}