using System.ComponentModel.DataAnnotations;

namespace SRMP.Models
{
    public class JobVacancy
    {
        public int JobVacancyId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string RequiredSkills { get; set; } = string.Empty;

        public int RequiredExperience { get; set; }

        public string RequiredEducation { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Location { get; set; } = string.Empty;

        public int EmployerId { get; set; }

        public bool IsOpen { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}