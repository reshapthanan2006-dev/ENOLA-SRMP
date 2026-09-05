using Microsoft.EntityFrameworkCore;
using SRMP.Data;
using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Repositories
{
    public class EmployerCompanyRepository : IEmployerCompanyRepository
    {
        private readonly AppDbContext _context;

        public EmployerCompanyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EmployerCompany?> GetByEmployerIdAsync(int employerId)
        {
            return await _context.EmployerCompanies
                .FirstOrDefaultAsync(c => c.EmployerId == employerId);
        }

        public async Task<EmployerCompany?> GetByIdAsync(int id)
        {
            return await _context.EmployerCompanies
                .FirstOrDefaultAsync(c => c.EmployerCompanyId == id);
        }

        public async Task AddAsync(EmployerCompany company)
        {
            await _context.EmployerCompanies.AddAsync(company);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EmployerCompany company)
        {
            _context.EmployerCompanies.Update(company);
            await _context.SaveChangesAsync();
        }
    }
}