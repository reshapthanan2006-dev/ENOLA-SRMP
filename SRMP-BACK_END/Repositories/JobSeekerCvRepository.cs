using Microsoft.EntityFrameworkCore;
using SRMP.Data;
using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Repositories
{
    public class JobSeekerCvRepository : IJobSeekerCvRepository
    {
        private readonly AppDbContext _context;

        public JobSeekerCvRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JobSeekerCv?> GetByUserIdAsync(int userId)
        {
            return await _context.JobSeekerCvs
                .FirstOrDefaultAsync(cv => cv.UserId == userId);
        }

        public async Task AddAsync(JobSeekerCv cv)
        {
            await _context.JobSeekerCvs.AddAsync(cv);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobSeekerCv cv)
        {
            var existingCv = await _context.JobSeekerCvs
                .FirstOrDefaultAsync(x => x.UserId == cv.UserId);

            if (existingCv == null)
                throw new KeyNotFoundException("CV not found.");

            existingCv.OriginalFileName = cv.OriginalFileName;
            existingCv.StoredFileName = cv.StoredFileName;
            existingCv.FilePath = cv.FilePath;
            existingCv.ContentType = cv.ContentType;
            existingCv.FileSize = cv.FileSize;
            existingCv.UploadedAt = cv.UploadedAt;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(JobSeekerCv cv)
        {
            _context.JobSeekerCvs.Remove(cv);
            await _context.SaveChangesAsync();
        }
    }
}