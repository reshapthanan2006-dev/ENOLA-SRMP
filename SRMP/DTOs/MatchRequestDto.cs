using SRMP.Models;

namespace SRMP.DTOs
{
    public class MatchRequestDto
    {
        public JobSeekerProfile Profile { get; set; } = new();

        public Vacancy Vacancy { get; set; } = new();
    }
}