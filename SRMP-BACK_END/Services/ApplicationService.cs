using SRMP.DTOs;
using SRMP.Interfaces;
using SRMP.Interfaces.Services;
using SRMP.Models;

namespace SRMP.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly INotificationService _notificationService;
        private readonly IJobVacancyService _jobVacancyService;
        private readonly IJobSeekerProfileRepository _jobSeekerProfileRepository;
        private readonly IMatchingService _matchingService;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            INotificationService notificationService,
            IJobVacancyService jobVacancyService,
            IJobSeekerProfileRepository jobSeekerProfileRepository,
            IMatchingService matchingService)
        {
            _applicationRepository = applicationRepository;
            _notificationService = notificationService;
            _jobVacancyService = jobVacancyService;
            _jobSeekerProfileRepository = jobSeekerProfileRepository;
            _matchingService = matchingService;
        }

        // -------------------------------------------------
        // Job Seeker applies for a vacancy
        // -------------------------------------------------
        public async Task<ApplicationResponseDto> CreateApplicationAsync(
            int jobSeekerId,
            CreateApplicationDto dto)
        {
            // Check vacancy exists
            var vacancy =
                await _jobVacancyService.GetByIdAsync(
                    dto.JobVacancyId);

            if (vacancy == null)
            {
                throw new KeyNotFoundException(
                    "Vacancy not found.");
            }

            // Job Seeker can apply only to open vacancy
            if (!vacancy.IsOpen)
            {
                throw new InvalidOperationException(
                    "This vacancy is closed. You cannot apply.");
            }

            // Check duplicate application
            var existingApplication =
                await _applicationRepository
                    .GetByJobSeekerAndVacancyAsync(
                        jobSeekerId,
                        dto.JobVacancyId);

            if (existingApplication != null)
            {
                throw new InvalidOperationException(
                    "You have already applied for this job.");
            }

            // Create application
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

        // -------------------------------------------------
        // Job Seeker views own applications
        // -------------------------------------------------
        public async Task<List<ApplicationResponseDto>>
            GetMyApplicationsAsync(int jobSeekerId)
        {
            var applications =
                await _applicationRepository
                    .GetByJobSeekerAsync(jobSeekerId);

            return applications
                .Select(application =>
                    new ApplicationResponseDto
                    {
                        ApplicationId =
                            application.ApplicationId,

                        JobSeekerId =
                            application.JobSeekerId,

                        JobVacancyId =
                            application.JobVacancyId,

                        Status =
                            application.Status,

                        AppliedAt =
                            application.AppliedAt
                    })
                .ToList();
        }

        // -------------------------------------------------
        // Employer views applications for own vacancy
        // -------------------------------------------------
        public async Task<List<ApplicationResponseDto>>
            GetApplicationsByVacancyAsync(
                int employerId,
                int jobVacancyId)
        {
            var vacancy =
                await _jobVacancyService.GetByIdAsync(
                    jobVacancyId);

            if (vacancy == null)
            {
                throw new KeyNotFoundException(
                    "Vacancy not found.");
            }

            // Employer must own vacancy
            if (vacancy.EmployerId != employerId)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to view applicants for this vacancy.");
            }

            var applications =
                await _applicationRepository
                    .GetByVacancyAsync(jobVacancyId);

            return applications
                .Select(application =>
                    new ApplicationResponseDto
                    {
                        ApplicationId =
                            application.ApplicationId,

                        JobSeekerId =
                            application.JobSeekerId,

                        JobVacancyId =
                            application.JobVacancyId,

                        Status =
                            application.Status,

                        AppliedAt =
                            application.AppliedAt
                    })
                .ToList();
        }

        // -------------------------------------------------
        // Employer updates application status
        // -------------------------------------------------
        public async Task<ApplicationResponseDto?>
            UpdateApplicationStatusAsync(
                int employerId,
                int applicationId,
                UpdateApplicationStatusDto dto)
        {
            var application =
                await _applicationRepository
                    .GetByIdAsync(applicationId);

            if (application == null)
            {
                return null;
            }

            var vacancy =
                await _jobVacancyService.GetByIdAsync(
                    application.JobVacancyId);

            if (vacancy == null)
            {
                return null;
            }

            // Employer must own the vacancy
            if (vacancy.EmployerId != employerId)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to update this application.");
            }

            // Allowed application statuses
            var allowedStatuses = new[]
            {
                "Pending",
                "Shortlisted",
                "Rejected",
                "Accepted"
            };

            var newStatus =
                allowedStatuses.FirstOrDefault(
                    status => string.Equals(
                        status,
                        dto.Status?.Trim(),
                        StringComparison.OrdinalIgnoreCase));

            if (newStatus == null)
            {
                throw new ArgumentException(
                    "Invalid status. Allowed statuses are: Pending, Shortlisted, Rejected, Accepted.");
            }

            application.Status = newStatus;

            await _applicationRepository
                .UpdateAsync(application);

            // Notify Job Seeker
            await _notificationService
                .CreateNotificationAsync(
                    application.JobSeekerId,
                    application.ApplicationId,
                    $"Your application status has been updated to {application.Status}."
                );

            return new ApplicationResponseDto
            {
                ApplicationId =
                    application.ApplicationId,

                JobSeekerId =
                    application.JobSeekerId,

                JobVacancyId =
                    application.JobVacancyId,

                Status =
                    application.Status,

                AppliedAt =
                    application.AppliedAt
            };
        }

        // -------------------------------------------------
        // Employer views ranked applicants
        // -------------------------------------------------
        public async Task<List<RankedApplicantResponseDto>>
            GetRankedApplicantsAsync(
                int employerId,
                int jobVacancyId)
        {
            // Get vacancy
            var jobVacancy =
                await _jobVacancyService
                    .GetByIdAsync(jobVacancyId);

            if (jobVacancy == null)
            {
                throw new KeyNotFoundException(
                    "Vacancy not found.");
            }

            // Employer must own the vacancy
            if (jobVacancy.EmployerId != employerId)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to view applicants for this vacancy.");
            }

            // Get actual applications
            var applications =
                await _applicationRepository
                    .GetByVacancyAsync(jobVacancyId);

            var rankedApplicants =
                new List<RankedApplicantResponseDto>();

            foreach (var application in applications)
            {
                // Get Job Seeker profile
                var profile =
                    await _jobSeekerProfileRepository
                        .GetByUserIdAsync(
                            application.JobSeekerId);

                // Skip if profile does not exist
                if (profile == null)
                {
                    continue;
                }

                // Convert JobVacancy to Vacancy
                // used by MatchingService
                var vacancy = new Vacancy
                {
                    Id = jobVacancy.JobVacancyId,

                    RequiredSkills =
                        jobVacancy.RequiredSkills
                            .Split(
                                ',',
                                StringSplitOptions.RemoveEmptyEntries |
                                StringSplitOptions.TrimEntries)
                            .ToList(),

                    RequiredExperienceYears =
                        jobVacancy.RequiredExperience,

                    RequiredEducation =
                        jobVacancy.RequiredEducation,

                    Location =
                        jobVacancy.Location
                };

                // Calculate match using backend MatchingService
                var matchResult =
                    _matchingService.CalculateMatch(
                        profile,
                        vacancy);

                rankedApplicants.Add(
                    new RankedApplicantResponseDto
                    {
                        ApplicationId =
                            application.ApplicationId,

                        JobSeekerId =
                            application.JobSeekerId,

                        JobVacancyId =
                            application.JobVacancyId,

                        Status =
                            application.Status,

                        AppliedAt =
                            application.AppliedAt,

                        MatchScore =
                            matchResult.MatchScore,

                        SkillsScore =
                            matchResult.SkillsScore,

                        ExperienceScore =
                            matchResult.ExperienceScore,

                        EducationScore =
                            matchResult.EducationScore,

                        LocationScore =
                            matchResult.LocationScore,

                        MissingSkills =
                            matchResult.MissingSkills
                    });
            }

            // Ranking order:
            // 1. Match Score
            // 2. Skills Score
            // 3. Experience Score
            // 4. Education Score
            // 5. Location Score
            // 6. Earlier Application Date
            return rankedApplicants
                .OrderByDescending(
                    applicant => applicant.MatchScore)
                .ThenByDescending(
                    applicant => applicant.SkillsScore)
                .ThenByDescending(
                    applicant => applicant.ExperienceScore)
                .ThenByDescending(
                    applicant => applicant.EducationScore)
                .ThenByDescending(
                    applicant => applicant.LocationScore)
                .ThenBy(
                    applicant => applicant.AppliedAt)
                .ToList();
        }
    }
}