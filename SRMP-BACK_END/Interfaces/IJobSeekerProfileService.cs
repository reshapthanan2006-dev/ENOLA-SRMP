using SRMP.DTOs.JobSeeker;

namespace SRMP.Interfaces
{
    public interface IJobSeekerProfileService
    {
        Task<JobSeekerProfileResponseDto?> GetProfileAsync(int userId);

        Task<JobSeekerProfileResponseDto> CreateProfileAsync(
            int userId,
            CreateJobSeekerProfileDto dto);

        Task<JobSeekerProfileResponseDto> UpdateProfileAsync(
            int userId,
            CreateJobSeekerProfileDto dto);
    }
}