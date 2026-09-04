namespace SRMP.Models
{
    public class MatchResult
    {
        public int CandidateId { get; set; }
        public int VacancyId { get; set; }
        public double MatchScore { get; set; }
        public List<string> MissingSkills { get; set; } = new ();
    }
}
