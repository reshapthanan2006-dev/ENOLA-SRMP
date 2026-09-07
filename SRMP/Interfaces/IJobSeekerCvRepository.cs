using SRMP.Models;

namespace SRMP.Interfaces
{
    public interface IJobSeekerCvRepository
    {
        Task<JobSeekerCv?> GetByUserIdAsync(int userId);

        Task AddAsync(JobSeekerCv cv);

        Task UpdateAsync(JobSeekerCv cv);

        Task DeleteAsync(JobSeekerCv cv);
    }
}