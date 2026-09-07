using SRMP.Models;

namespace SRMP.Interfaces
{
    public interface IJobVacancyRepository
    {
        Task<JobVacancy?> GetByIdAsync(int id);

        Task<List<JobVacancy>> GetByEmployerIdAsync(int employerId);

        Task<List<JobVacancy>> SearchOpenVacanciesAsync(
                string? keyword,
                string? location,
                int? minExperience);

        Task AddAsync(JobVacancy vacancy);

        Task UpdateAsync(JobVacancy vacancy);
    }
}