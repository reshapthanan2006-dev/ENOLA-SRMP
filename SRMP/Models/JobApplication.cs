using System.ComponentModel.DataAnnotations;

namespace SRMP.Models
{
    public class JobApplication
    {
        [Key]
        public int JobApplicationId { get; set; }

        public int JobVacancyId { get; set; }

        public int JobSeekerId { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    }
}