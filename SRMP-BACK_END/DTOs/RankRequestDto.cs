using SRMP.Models;

namespace SRMP.DTOs
{
    public class RankRequestDto
    {
        public List<JobSeekerProfile> Candidates { get; set; } = new();

        public Vacancy Vacancy { get; set; } = new();
    }
}