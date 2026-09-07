using SRMP.DTOs.JobSeeker;
using SRMP.Interfaces;
using SRMP.Models;

namespace SRMP.Services
{
    public class JobSeekerProfileService : IJobSeekerProfileService
    {
        private readonly IJobSeekerProfileRepository _repository;

        public JobSeekerProfileService(
            IJobSeekerProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<JobSeekerProfileResponseDto?> GetProfileAsync(
            int userId)
        {
            var profile = await _repository.GetByUserIdAsync(userId);

            if (profile == null)
                return null;

            return MapToResponseDto(profile);
        }

        public async Task<JobSeekerProfileResponseDto> CreateProfileAsync(
            int userId,
            CreateJobSeekerProfileDto dto)
        {
            var existingProfile =
                await _repository.GetByUserIdAsync(userId);

            if (existingProfile != null)
                throw new InvalidOperationException(
                    "Job seeker profile already exists.");

            var profile = new JobSeekerProfile
            {
                UserId = userId,
                Skills = dto.Skills,
                ExperienceYears = dto.ExperienceYears,
                Education = dto.Education,
                Location = dto.Location
            };

            await _repository.AddAsync(profile);

            return MapToResponseDto(profile);
        }

        public async Task<JobSeekerProfileResponseDto> UpdateProfileAsync(
            int userId,
            CreateJobSeekerProfileDto dto)
        {
            var profile =
                await _repository.GetByUserIdAsync(userId);

            if (profile == null)
                throw new KeyNotFoundException(
                    "Job seeker profile not found.");

            profile.Skills = dto.Skills;
            profile.ExperienceYears = dto.ExperienceYears;
            profile.Education = dto.Education;
            profile.Location = dto.Location;

            await _repository.UpdateAsync(profile);

            return MapToResponseDto(profile);
        }

        private static JobSeekerProfileResponseDto MapToResponseDto(
            JobSeekerProfile profile)
        {
            return new JobSeekerProfileResponseDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                Skills = profile.Skills,
                ExperienceYears = profile.ExperienceYears,
                Education = profile.Education,
                Location = profile.Location
            };
        }
    }
}