using SRMP.Models;

namespace SRMP.Interfaces
{
    public interface IJobVacancyService
    {
        Task<JobVacancy?> GetByIdAsync(int id);

        Task<List<JobVacancy>> GetByEmployerIdAsync(int employerId);

        Task CreateAsync(JobVacancy vacancy);

        Task UpdateAsync(JobVacancy vacancy);

        Task CloseAsync(int id, int employerId);
    }
}