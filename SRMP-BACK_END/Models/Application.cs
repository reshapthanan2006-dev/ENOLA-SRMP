using System.ComponentModel.DataAnnotations;

namespace SRMP.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }

        [Required]
        public int JobSeekerId { get; set; }

        [Required]
        public int JobVacancyId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    }
}