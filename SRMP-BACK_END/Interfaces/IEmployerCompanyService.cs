using SRMP.Models;

namespace SRMP.Interfaces
{
    public interface IEmployerCompanyService
    {
        Task<EmployerCompany?> GetCompanyByEmployerIdAsync(int employerId);

        Task<EmployerCompany?> GetCompanyByIdAsync(int id);

        Task CreateCompanyAsync(EmployerCompany company);

        Task UpdateCompanyAsync(EmployerCompany company);
    }
}