using Microsoft.EntityFrameworkCore;
using SRMP.Data;
using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Repositories
{
    public class JobSeekerProfileRepository : IJobSeekerProfileRepository
    {
        private readonly AppDbContext _context;

        public JobSeekerProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JobSeekerProfile?> GetByUserIdAsync(int userId)
        {
            return await _context.JobSeekerProfiles
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task AddAsync(JobSeekerProfile profile)
        {
            await _context.JobSeekerProfiles.AddAsync(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobSeekerProfile profile)
        {
            var existingProfile = await _context.JobSeekerProfiles
                .FirstOrDefaultAsync(p => p.UserId == profile.UserId);

            if (existingProfile == null)
                throw new KeyNotFoundException("Job seeker profile not found.");

            existingProfile.Skills = profile.Skills;
            existingProfile.ExperienceYears = profile.ExperienceYears;
            existingProfile.Education = profile.Education;
            existingProfile.Location = profile.Location;

            await _context.SaveChangesAsync();
        }
    }
}