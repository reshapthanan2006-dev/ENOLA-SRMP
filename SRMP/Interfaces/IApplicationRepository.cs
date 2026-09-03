using SRMP.Models;

namespace SRMP.Interfaces
{
    public interface IApplicationRepository
    {
        Task<Application?> GetByIdAsync(int applicationId);

        Task<Application?> GetByJobSeekerAndVacancyAsync(
            int jobSeekerId,
            int jobVacancyId);

        Task<List<Application>> GetByJobSeekerAsync(int jobSeekerId);

        Task<List<Application>> GetByVacancyAsync(int jobVacancyId);

        Task AddAsync(Application application);

        Task UpdateAsync(Application application);
    }
}