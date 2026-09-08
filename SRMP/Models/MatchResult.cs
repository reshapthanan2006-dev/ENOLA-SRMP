namespace SRMP.Models
{
    public class MatchResult
    {
        public int CandidateId { get; set; }

        public int VacancyId { get; set; }

        public double MatchScore { get; set; }

        public double SkillsScore { get; set; }

        public double ExperienceScore { get; set; }

        public double EducationScore { get; set; }

        public double LocationScore { get; set; }

        public List<string> MissingSkills { get; set; } = new();
    }
}