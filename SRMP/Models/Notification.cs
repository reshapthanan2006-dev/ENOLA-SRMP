using System.ComponentModel.DataAnnotations;

namespace SRMP.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        [Required]
        public int JobSeekerId { get; set; }

        [Required]
        public int ApplicationId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}