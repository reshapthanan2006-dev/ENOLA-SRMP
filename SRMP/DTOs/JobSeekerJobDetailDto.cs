namespace SRMP.DTOs.JobSeeker
{
    public class JobSeekerJobDetailDto
    {
        public int JobVacancyId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string RequiredSkills { get; set; } = string.Empty;

        public int RequiredExperience { get; set; }

        public string Location { get; set; } = string.Empty;

        public bool IsOpen { get; set; }

        public double MatchScore { get; set; }

        public List<string> MissingSkills { get; set; } = new();
    }
}