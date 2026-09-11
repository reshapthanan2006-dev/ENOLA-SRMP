namespace SRMP.DTOs
{
    public class RankedApplicantResponseDto
    {
        public int ApplicationId { get; set; }

        public int JobSeekerId { get; set; }

        public int JobVacancyId { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime AppliedAt { get; set; }

        public double MatchScore { get; set; }

        public double SkillsScore { get; set; }

        public double ExperienceScore { get; set; }

        public double EducationScore { get; set; }

        public double LocationScore { get; set; }

        public List<string> MissingSkills { get; set; } = new();
    }
}