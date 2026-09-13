using System.ComponentModel.DataAnnotations;

namespace SRMP.DTOs
{
    public class UpdateUserStatusDto
    {
        [Required]
        public bool IsActive { get; set; }
    }
}