using SRMP.DTOs;
using SRMP.Interfaces;
using SRMP.Interfaces.Services;
using SRMP.Models;

namespace SRMP.Services
{
    public class ContactRequestService : IContactRequestService
    {
        private readonly IContactRequestRepository
            _contactRequestRepository;

        private readonly IApplicationRepository
            _applicationRepository;

        private readonly IJobVacancyService
            _jobVacancyService;

        private readonly IEmployerCompanyService
            _employerCompanyService;

        public ContactRequestService(
            IContactRequestRepository contactRequestRepository,
            IApplicationRepository applicationRepository,
            IJobVacancyService jobVacancyService,
            IEmployerCompanyService employerCompanyService)
        {
            _contactRequestRepository =
                contactRequestRepository;

            _applicationRepository =
                applicationRepository;

            _jobVacancyService =
                jobVacancyService;

            _employerCompanyService =
                employerCompanyService;
        }

        public async Task<ContactRequestResponseDto>
            CreateContactRequestAsync(
                int employerId,
                CreateContactRequestDto dto)
        {
            var application =
                await _applicationRepository
                    .GetByIdAsync(
                        dto.ApplicationId
                    );

            if (application == null)
            {
                throw new KeyNotFoundException(
                    "Application not found."
                );
            }

            var vacancy =
                await _jobVacancyService
                    .GetByIdAsync(
                        application.JobVacancyId
                    );

            if (vacancy == null)
            {
                throw new KeyNotFoundException(
                    "Vacancy not found."
                );
            }

            if (vacancy.EmployerId != employerId)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to send a contact request for this application."
                );
            }

            if (
                application.JobSeekerId !=
                dto.JobSeekerId
            )
            {
                throw new ArgumentException(
                    "Job Seeker does not match this application."
                );
            }

            var existingRequest =
                await _contactRequestRepository
                    .GetByApplicationAsync(
                        dto.ApplicationId
                    );

            if (existingRequest != null)
            {
                throw new InvalidOperationException(
                    "A contact request already exists for this application."
                );
            }

            var contactRequest =
                new ContactRequest
                {
                    EmployerId = employerId,

                    JobSeekerId =
                        application.JobSeekerId,

                    ApplicationId =
                        application.ApplicationId,

                    Status = "Pending",

                    CreatedAt =
                        DateTime.UtcNow
                };

            await _contactRequestRepository
                .AddAsync(contactRequest);

            return await MapToDtoAsync(
                contactRequest
            );
        }

        public async Task<List<ContactRequestResponseDto>>
            GetMyRequestsAsync(
                int jobSeekerId)
        {
            var requests =
                await _contactRequestRepository
                    .GetByJobSeekerAsync(
                        jobSeekerId
                    );

            var result =
                new List<ContactRequestResponseDto>();

            foreach (var request in requests)
            {
                result.Add(
                    await MapToDtoAsync(request)
                );
            }

            return result;
        }

        public async Task<List<ContactRequestResponseDto>>
            GetSentRequestsAsync(
                int employerId)
        {
            var requests =
                await _contactRequestRepository
                    .GetByEmployerAsync(
                        employerId
                    );

            var result =
                new List<ContactRequestResponseDto>();

            foreach (var request in requests)
            {
                result.Add(
                    await MapToDtoAsync(request)
                );
            }

            return result;
        }

        public async Task<ContactRequestResponseDto?>
            RespondToRequestAsync(
                int jobSeekerId,
                int contactRequestId,
                string status)
        {
            var request =
                await _contactRequestRepository
                    .GetByIdAsync(
                        contactRequestId
                    );

            if (request == null)
            {
                return null;
            }

            if (
                request.JobSeekerId !=
                jobSeekerId
            )
            {
                return null;
            }

            if (
                !string.Equals(
                    request.Status,
                    "Pending",
                    StringComparison.OrdinalIgnoreCase
                )
            )
            {
                throw new InvalidOperationException(
                    "This contact request has already been responded to."
                );
            }

            var allowedStatuses =
                new[]
                {
                    "Accepted",
                    "Declined"
                };

            var newStatus =
                allowedStatuses
                    .FirstOrDefault(
                        allowedStatus =>
                            string.Equals(
                                allowedStatus,
                                status?.Trim(),
                                StringComparison.OrdinalIgnoreCase
                            )
                    );

            if (newStatus == null)
            {
                throw new InvalidOperationException(
                    "Status must be Accepted or Declined."
                );
            }

            request.Status =
                newStatus;

            await _contactRequestRepository
                .UpdateAsync(request);

            return await MapToDtoAsync(
                request
            );
        }

        private async Task<ContactRequestResponseDto>
            MapToDtoAsync(
                ContactRequest request)
        {
            var company =
                await _employerCompanyService
                    .GetCompanyByEmployerIdAsync(
                        request.EmployerId
                    );

            var application =
                await _applicationRepository
                    .GetByIdAsync(
                        request.ApplicationId
                    );

            JobVacancy? vacancy = null;

            if (application != null)
            {
                vacancy =
                    await _jobVacancyService
                        .GetByIdAsync(
                            application.JobVacancyId
                        );
            }

            return new ContactRequestResponseDto
            {
                ContactRequestId =
                    request.ContactRequestId,

                EmployerId =
                    request.EmployerId,

                JobSeekerId =
                    request.JobSeekerId,

                ApplicationId =
                    request.ApplicationId,

                JobVacancyId =
                    application?.JobVacancyId ?? 0,

                CompanyName =
                    company?.CompanyName
                    ?? $"Employer #{request.EmployerId}",

                JobTitle =
                    vacancy?.Title
                    ?? "Job Vacancy",

                JobLocation =
                    vacancy?.Location
                    ?? string.Empty,

                ApplicationStatus =
                    application?.Status
                    ?? string.Empty,

                Status =
                    request.Status,

                CreatedAt =
                    request.CreatedAt
            };
        }
    }
}