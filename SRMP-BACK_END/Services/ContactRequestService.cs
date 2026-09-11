using SRMP.DTOs;
using SRMP.Interfaces;
using SRMP.Interfaces.Services;
using SRMP.Models;

namespace SRMP.Services
{
    public class ContactRequestService : IContactRequestService
    {
        private readonly IContactRequestRepository _contactRequestRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobVacancyService _jobVacancyService;

        public ContactRequestService(
            IContactRequestRepository contactRequestRepository,
            IApplicationRepository applicationRepository,
            IJobVacancyService jobVacancyService)
        {
            _contactRequestRepository = contactRequestRepository;
            _applicationRepository = applicationRepository;
            _jobVacancyService = jobVacancyService;
        }

        // Employer sends a contact request
        public async Task<ContactRequestResponseDto>
            CreateContactRequestAsync(
                int employerId,
                CreateContactRequestDto dto)
        {
            // Check application exists
            var application =
                await _applicationRepository.GetByIdAsync(
                    dto.ApplicationId);

            if (application == null)
            {
                throw new KeyNotFoundException(
                    "Application not found.");
            }

            // Check vacancy exists
            var vacancy =
                await _jobVacancyService.GetByIdAsync(
                    application.JobVacancyId);

            if (vacancy == null)
            {
                throw new KeyNotFoundException(
                    "Vacancy not found.");
            }

            // Employer must own the vacancy
            if (vacancy.EmployerId != employerId)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to send a contact request for this application.");
            }

            // JobSeekerId must match the actual applicant
            if (application.JobSeekerId != dto.JobSeekerId)
            {
                throw new ArgumentException(
                    "Job Seeker does not match this application.");
            }

            // Prevent duplicate contact request
            var existingRequest =
                await _contactRequestRepository
                    .GetByApplicationAsync(dto.ApplicationId);

            if (existingRequest != null)
            {
                throw new InvalidOperationException(
                    "A contact request already exists for this application.");
            }

            var contactRequest = new ContactRequest
            {
                EmployerId = employerId,

                // Use verified applicant
                JobSeekerId = application.JobSeekerId,

                ApplicationId = application.ApplicationId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _contactRequestRepository.AddAsync(
                contactRequest);

            return MapToDto(contactRequest);
        }

        // Job Seeker views received requests
        public async Task<List<ContactRequestResponseDto>>
            GetMyRequestsAsync(int jobSeekerId)
        {
            var requests =
                await _contactRequestRepository
                    .GetByJobSeekerAsync(jobSeekerId);

            return requests
                .Select(MapToDto)
                .ToList();
        }

        // Employer views sent requests
        public async Task<List<ContactRequestResponseDto>>
            GetSentRequestsAsync(int employerId)
        {
            var requests =
                await _contactRequestRepository
                    .GetByEmployerAsync(employerId);

            return requests
                .Select(MapToDto)
                .ToList();
        }

        // Job Seeker accepts or declines request
        public async Task<ContactRequestResponseDto?>
            RespondToRequestAsync(
                int jobSeekerId,
                int contactRequestId,
                string status)
        {
            var request =
                await _contactRequestRepository
                    .GetByIdAsync(contactRequestId);

            if (request == null)
            {
                return null;
            }

            // Only intended Job Seeker can respond
            if (request.JobSeekerId != jobSeekerId)
            {
                return null;
            }

            // Already responded request cannot be changed again
            if (!string.Equals(
                    request.Status,
                    "Pending",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "This contact request has already been responded to.");
            }

            var allowedStatuses = new[]
            {
                "Accepted",
                "Declined"
            };

            var newStatus =
                allowedStatuses.FirstOrDefault(
                    allowedStatus => string.Equals(
                        allowedStatus,
                        status?.Trim(),
                        StringComparison.OrdinalIgnoreCase));

            if (newStatus == null)
            {
                throw new InvalidOperationException(
                    "Status must be Accepted or Declined.");
            }

            request.Status = newStatus;

            await _contactRequestRepository
                .UpdateAsync(request);

            return MapToDto(request);
        }

        private static ContactRequestResponseDto MapToDto(
            ContactRequest request)
        {
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

                Status =
                    request.Status,

                CreatedAt =
                    request.CreatedAt
            };
        }
    }
}