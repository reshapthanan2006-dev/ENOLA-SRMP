using SRMP.Models;

namespace SRMP.Interfaces
{
    public interface IJobSeekerProfileRepository
    {
        Task<JobSeekerProfile?> GetByUserIdAsync(int userId);

        Task AddAsync(JobSeekerProfile profile);

        Task UpdateAsync(JobSeekerProfile profile);
    }
}