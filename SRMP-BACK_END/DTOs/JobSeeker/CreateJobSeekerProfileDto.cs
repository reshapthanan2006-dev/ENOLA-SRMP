using System.ComponentModel.DataAnnotations;

namespace SRMP.DTOs.JobSeeker
{
    public class CreateJobSeekerProfileDto
    {
        [Required]
        [MinLength(1)]
        public List<string> Skills { get; set; } = new();

        [Range(0, 50)]
        public int ExperienceYears { get; set; }

        [Required]
        [MaxLength(200)]
        public string Education { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Location { get; set; } = string.Empty;
    }
}