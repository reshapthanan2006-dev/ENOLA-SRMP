using SRMP.DTOs;
using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly INotificationService _notificationService;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            INotificationService notificationService)
        {
            _applicationRepository = applicationRepository;
            _notificationService = notificationService;
        }

        public async Task<ApplicationResponseDto> CreateApplicationAsync(
            int jobSeekerId,
            CreateApplicationDto dto)
        {
            var existingApplication =
                await _applicationRepository.GetByJobSeekerAndVacancyAsync(
                    jobSeekerId,
                    dto.JobVacancyId);

            if (existingApplication != null)
            {
                throw new InvalidOperationException(
                    "You have already applied for this job.");
            }

            var application = new Application
            {
                JobSeekerId = jobSeekerId,
                JobVacancyId = dto.JobVacancyId,
                Status = "Pending",
                AppliedAt = DateTime.UtcNow
            };

            await _applicationRepository.AddAsync(application);

            return new ApplicationResponseDto
            {
                ApplicationId = application.ApplicationId,
                JobSeekerId = application.JobSeekerId,
                JobVacancyId = application.JobVacancyId,
                Status = application.Status,
                AppliedAt = application.AppliedAt
            };
        }

        public async Task<List<ApplicationResponseDto>> GetMyApplicationsAsync(
            int jobSeekerId)
        {
            var applications =
                await _applicationRepository.GetByJobSeekerAsync(jobSeekerId);

            return applications.Select(a => new ApplicationResponseDto
            {
                ApplicationId = a.ApplicationId,
                JobSeekerId = a.JobSeekerId,
                JobVacancyId = a.JobVacancyId,
                Status = a.Status,
                AppliedAt = a.AppliedAt
            }).ToList();
        }

        public async Task<List<ApplicationResponseDto>> GetApplicationsByVacancyAsync(
            int employerId,
            int jobVacancyId)
        {
            // Employer ownership validation will be added
            // when JobVacancy repository/service is integrated.

            var applications =
                await _applicationRepository.GetByVacancyAsync(jobVacancyId);

            return applications.Select(a => new ApplicationResponseDto
            {
                ApplicationId = a.ApplicationId,
                JobSeekerId = a.JobSeekerId,
                JobVacancyId = a.JobVacancyId,
                Status = a.Status,
                AppliedAt = a.AppliedAt
            }).ToList();
        }

        public async Task<ApplicationResponseDto?> UpdateApplicationStatusAsync(
            int employerId,
            int applicationId,
            UpdateApplicationStatusDto dto)
        {
            var application =
                await _applicationRepository.GetByIdAsync(applicationId);

            if (application == null)
            {
                return null;
            }

            application.Status = dto.Status;

            await _applicationRepository.UpdateAsync(application);

            await _notificationService.CreateNotificationAsync(
                application.JobSeekerId,
                application.ApplicationId,
                $"Your application status has been updated to {application.Status}."
            );

            return new ApplicationResponseDto
            {
                ApplicationId = application.ApplicationId,
                JobSeekerId = application.JobSeekerId,
                JobVacancyId = application.JobVacancyId,
                Status = application.Status,
                AppliedAt = application.AppliedAt
            };
        }
    }
}