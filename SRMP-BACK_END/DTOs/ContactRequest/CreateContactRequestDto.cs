using System.ComponentModel.DataAnnotations;

namespace SRMP.DTOs
{
    public class CreateContactRequestDto
    {
        [Required]
        public int JobSeekerId { get; set; }

        [Required]
        public int ApplicationId { get; set; }
    }
}