using System.ComponentModel.DataAnnotations;

namespace SRMP.DTOs
{
    public class CreateApplicationDto
    {
        [Required]
        public int JobVacancyId { get; set; }
    }
}