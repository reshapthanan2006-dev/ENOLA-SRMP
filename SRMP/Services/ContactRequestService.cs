using SRMP.DTOs;
using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Services
{
    public class ContactRequestService : IContactRequestService
    {
        private readonly IContactRequestRepository _contactRequestRepository;

        public ContactRequestService(
            IContactRequestRepository contactRequestRepository)
        {
            _contactRequestRepository = contactRequestRepository;
        }

        public async Task<ContactRequestResponseDto> CreateContactRequestAsync(
            int employerId,
            CreateContactRequestDto dto)
        {
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
                JobSeekerId = dto.JobSeekerId,
                ApplicationId = dto.ApplicationId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            await _contactRequestRepository.AddAsync(contactRequest);

            return MapToDto(contactRequest);
        }

        public async Task<List<ContactRequestResponseDto>> GetMyRequestsAsync(
            int jobSeekerId)
        {
            var requests =
                await _contactRequestRepository
                    .GetByJobSeekerAsync(jobSeekerId);

            return requests.Select(MapToDto).ToList();
        }

        public async Task<List<ContactRequestResponseDto>> GetSentRequestsAsync(
            int employerId)
        {
            var requests =
                await _contactRequestRepository
                    .GetByEmployerAsync(employerId);

            return requests.Select(MapToDto).ToList();
        }

        public async Task<ContactRequestResponseDto?> RespondToRequestAsync(
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

            // Only the intended Job Seeker can respond
            if (request.JobSeekerId != jobSeekerId)
            {
                return null;
            }

            if (status != "Accepted" && status != "Declined")
            {
                throw new InvalidOperationException(
                    "Status must be Accepted or Declined.");
            }

            request.Status = status;

            await _contactRequestRepository.UpdateAsync(request);

            return MapToDto(request);
        }

        private static ContactRequestResponseDto MapToDto(
            ContactRequest request)
        {
            return new ContactRequestResponseDto
            {
                ContactRequestId = request.ContactRequestId,
                EmployerId = request.EmployerId,
                JobSeekerId = request.JobSeekerId,
                ApplicationId = request.ApplicationId,
                Status = request.Status,
                CreatedAt = request.CreatedAt
            };
        }
    }
}