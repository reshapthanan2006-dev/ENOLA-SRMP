using Microsoft.AspNetCore.Http;

namespace SRMP.DTOs.JobSeeker
{
    public class UploadCvDto
    {
        public IFormFile File { get; set; } = null!;
    }
}