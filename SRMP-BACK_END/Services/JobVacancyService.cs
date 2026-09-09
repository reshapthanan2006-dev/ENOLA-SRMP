using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Services
{
    public class JobVacancyService : IJobVacancyService
    {
        private readonly IJobVacancyRepository _repository;

        public JobVacancyService(IJobVacancyRepository repository)
        {
            _repository = repository;
        }

        public async Task<JobVacancy?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<JobVacancy>> GetByEmployerIdAsync(
            int employerId)
        {
            return await _repository.GetByEmployerIdAsync(employerId);
        }

        public async Task<List<JobVacancy>> SearchOpenVacanciesAsync(
            string? keyword,
            string? location,
            int? minExperience)
        {
            return await _repository.SearchOpenVacanciesAsync(
                keyword,
                location,
                minExperience);
        }

        public async Task CreateAsync(JobVacancy vacancy)
        {
            await _repository.AddAsync(vacancy);
        }

        public async Task UpdateAsync(JobVacancy vacancy)
        {
            await _repository.UpdateAsync(vacancy);
        }

        public async Task CloseAsync(int id, int employerId)
        {
            var vacancy = await _repository.GetByIdAsync(id);

            if (vacancy == null)
                throw new KeyNotFoundException("Vacancy not found.");

            if (vacancy.EmployerId != employerId)
                throw new UnauthorizedAccessException(
                    "You are not allowed to close this vacancy.");

            vacancy.IsOpen = false;

            await _repository.UpdateAsync(vacancy);
        }
    }
}