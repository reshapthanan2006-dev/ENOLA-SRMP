using SRMP.Models;

namespace SRMP.Interfaces
{
    public interface IJobVacancyRepository
    {
        Task<JobVacancy?> GetByIdAsync(int id);

        Task<List<JobVacancy>> GetByEmployerIdAsync(int employerId);

        Task AddAsync(JobVacancy vacancy);

        Task UpdateAsync(JobVacancy vacancy);
    }
}