using System.ComponentModel.DataAnnotations;

namespace SRMP.Models
{
    public class ContactRequest
    {
        public int ContactRequestId { get; set; }

        [Required]
        public int EmployerId { get; set; }

        [Required]
        public int JobSeekerId { get; set; }

        [Required]
        public int ApplicationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}