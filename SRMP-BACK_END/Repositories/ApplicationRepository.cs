using Microsoft.EntityFrameworkCore;
using SRMP.Data;
using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Application?> GetByIdAsync(int applicationId)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);
        }

        public async Task<Application?> GetByJobSeekerAndVacancyAsync(
            int jobSeekerId,
            int jobVacancyId)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(a =>
                    a.JobSeekerId == jobSeekerId &&
                    a.JobVacancyId == jobVacancyId);
        }

        public async Task<List<Application>> GetByJobSeekerAsync(int jobSeekerId)
        {
            return await _context.Applications
                .Where(a => a.JobSeekerId == jobSeekerId)
                .OrderByDescending(a => a.AppliedAt)
                .ToListAsync();
        }

        public async Task<List<Application>> GetByVacancyAsync(int jobVacancyId)
        {
            return await _context.Applications
                .Where(a => a.JobVacancyId == jobVacancyId)
                .OrderByDescending(a => a.AppliedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Application application)
        {
            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Application application)
        {
            _context.Applications.Update(application);
            await _context.SaveChangesAsync();
        }
    }
}