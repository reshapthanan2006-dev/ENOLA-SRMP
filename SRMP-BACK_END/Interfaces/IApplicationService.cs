using SRMP.DTOs;

namespace SRMP.Interfaces
{
    public interface IApplicationService
    {
        Task<ApplicationResponseDto> CreateApplicationAsync(
            int jobSeekerId,
            CreateApplicationDto dto);

        Task<List<ApplicationResponseDto>> GetMyApplicationsAsync(
            int jobSeekerId);

        Task<List<ApplicationResponseDto>> GetApplicationsByVacancyAsync(
            int employerId,
            int jobVacancyId);

        Task<ApplicationResponseDto?> UpdateApplicationStatusAsync(
            int employerId,
            int applicationId,
            UpdateApplicationStatusDto dto);

        Task<List<RankedApplicantResponseDto>>
            GetRankedApplicantsAsync(
                int employerId,
                int jobVacancyId);
    }
}