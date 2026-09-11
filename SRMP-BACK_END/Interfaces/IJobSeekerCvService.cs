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

        Task<(
            byte[] FileBytes,
            string ContentType,
            string FileName)?> DownloadCvAsync(int userId);

        Task DeleteCvAsync(int userId);
    }
}