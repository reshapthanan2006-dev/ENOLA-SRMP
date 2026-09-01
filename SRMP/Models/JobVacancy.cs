using System.ComponentModel.DataAnnotations;

namespace SRMP.Models
{
    public class JobVacancy
    {
        [Key]
        public int JobVacancyId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public int EmployerId { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Open";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}