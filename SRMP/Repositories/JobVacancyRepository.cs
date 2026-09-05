using Microsoft.EntityFrameworkCore;
using SRMP.Data;
using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Repositories
{
    public class JobVacancyRepository : IJobVacancyRepository
    {
        private readonly AppDbContext _context;

        public JobVacancyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JobVacancy?> GetByIdAsync(int id)
        {
            return await _context.JobVacancies
                .FirstOrDefaultAsync(v => v.JobVacancyId == id);
        }

        public async Task<List<JobVacancy>> GetByEmployerIdAsync(int employerId)
        {
            return await _context.JobVacancies
                .Where(v => v.EmployerId == employerId)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(JobVacancy vacancy)
        {
            await _context.JobVacancies.AddAsync(vacancy);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobVacancy vacancy)
        {
            var existingVacancy = await _context.JobVacancies
                .FirstOrDefaultAsync(v => v.JobVacancyId == vacancy.JobVacancyId);

            if (existingVacancy == null)
                throw new KeyNotFoundException("Vacancy not found.");

            existingVacancy.Title = vacancy.Title;
            existingVacancy.Description = vacancy.Description;
            existingVacancy.RequiredSkills = vacancy.RequiredSkills;
            existingVacancy.RequiredExperience = vacancy.RequiredExperience;
            existingVacancy.Location = vacancy.Location;
            existingVacancy.EmployerId = vacancy.EmployerId;
            existingVacancy.IsOpen = vacancy.IsOpen;

            await _context.SaveChangesAsync();
        }
    }
}