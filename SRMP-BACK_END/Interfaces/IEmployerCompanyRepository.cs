using SRMP.Models;

namespace SRMP.Interfaces
{
    public interface IEmployerCompanyRepository
    {
        Task<EmployerCompany?> GetByEmployerIdAsync(int employerId);

        Task<EmployerCompany?> GetByIdAsync(int id);

        Task AddAsync(EmployerCompany company);

        Task UpdateAsync(EmployerCompany company);
    }
}