using Microsoft.AspNetCore.Http;
using SRMP.DTOs.JobSeeker;
using SRMP.Interfaces;
using SRMP.Interfaces.Services;
using SRMP.Models;

namespace SRMP.Services
{
    public class JobSeekerCvService : IJobSeekerCvService
    {
        private readonly IJobSeekerCvRepository _repository;

        public JobSeekerCvService(
            IJobSeekerCvRepository repository)
        {
            _repository = repository;
        }

        public async Task<JobSeekerCvResponseDto?> GetCvAsync(int userId)
        {
            var cv = await _repository.GetByUserIdAsync(userId);

            if (cv == null)
                return null;

            return MapToResponseDto(cv);
        }

        public async Task<JobSeekerCvResponseDto> UploadCvAsync(
            int userId,
            IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("CV file is required.");

            const long maxFileSize = 5 * 1024 * 1024;

            if (file.Length > maxFileSize)
                throw new ArgumentException("CV file size must not exceed 5 MB.");

            var allowedExtensions = new[]
            {
                    ".pdf",
                    ".doc",
                    ".docx"
};

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException(
                    "Only PDF, DOC, and DOCX files are allowed.");

            var existingCv = await _repository.GetByUserIdAsync(userId);

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "ProtectedCVs");

            Directory.CreateDirectory(uploadsFolder);

            var storedFileName =
                $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(
                uploadsFolder,
                storedFileName);

            await using (var stream = new FileStream(
                filePath,
                FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var cv = new JobSeekerCv
            {
                UserId = userId,
                OriginalFileName = file.FileName,
                StoredFileName = storedFileName,
                FilePath = filePath,
                ContentType = file.ContentType,
                FileSize = file.Length,
                UploadedAt = DateTime.UtcNow
            };

            if (existingCv == null)
            {
                await _repository.AddAsync(cv);
            }
            else
            {
                if (File.Exists(existingCv.FilePath))
                {
                    File.Delete(existingCv.FilePath);
                }

                cv.Id = existingCv.Id;

                await _repository.UpdateAsync(cv);
            }

            return MapToResponseDto(cv);
        }

        public async Task DeleteCvAsync(int userId)
        {
            var cv = await _repository.GetByUserIdAsync(userId);

            if (cv == null)
                throw new KeyNotFoundException("CV not found.");

            if (File.Exists(cv.FilePath))
            {
                File.Delete(cv.FilePath);
            }

            await _repository.DeleteAsync(cv);
        }

        private static JobSeekerCvResponseDto MapToResponseDto(
            JobSeekerCv cv)
        {
            return new JobSeekerCvResponseDto
            {
                Id = cv.Id,
                UserId = cv.UserId,
                OriginalFileName = cv.OriginalFileName,
                ContentType = cv.ContentType,
                FileSize = cv.FileSize,
                UploadedAt = cv.UploadedAt
            };
        }
    }
}