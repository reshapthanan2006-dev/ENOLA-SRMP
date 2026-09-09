using SRMP.DTOs;

namespace SRMP.Interfaces
{
    public interface IContactRequestService
    {
        Task<ContactRequestResponseDto> CreateContactRequestAsync(
            int employerId,
            CreateContactRequestDto dto);

        Task<List<ContactRequestResponseDto>> GetMyRequestsAsync(
            int jobSeekerId);

        Task<List<ContactRequestResponseDto>> GetSentRequestsAsync(
            int employerId);

        Task<ContactRequestResponseDto?> RespondToRequestAsync(
            int jobSeekerId,
            int contactRequestId,
            string status);
    }
}