using Microsoft.AspNetCore.Http;
using SRMP.DTOs.JobSeeker;

namespace SRMP.Interfaces.Services
{
    public interface IJobSeekerCvService
    {
        Task<JobSeekerCvResponseDto?> GetCvAsync(int userId);

        Task<JobSeekerCvResponseDto> UploadCvAsync(
            int userId,
            IFormFile file);

        Task DeleteCvAsync(int userId);
    }
}