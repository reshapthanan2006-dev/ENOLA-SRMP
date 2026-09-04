using SRMP.Models;

namespace SRMP.DTOs
{
    public class MatchRequest
    {
        public JobSeekerProfile Profile { get; set; } = new();

        public Vacancy Vacancy { get; set; } = new();
    }
}