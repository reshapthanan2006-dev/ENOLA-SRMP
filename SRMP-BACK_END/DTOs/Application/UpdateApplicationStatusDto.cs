using System.ComponentModel.DataAnnotations;

namespace SRMP.DTOs
{
    public class UpdateApplicationStatusDto
    {
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
    }
}