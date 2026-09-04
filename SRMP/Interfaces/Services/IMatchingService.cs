using SRMP.Models;

namespace SRMP.Interfaces.Services
{
    public interface IMatchingService
    {
        //Match score + result
        MatchResult CalculateMatch(
            JobSeekerProfile profile,
            Vacancy vacancy);

        //Missing required skills
        List<string> GetMissingSkills(
            JobSeekerProfile profile,
            Vacancy vacancy);

        //highest score first
        List<MatchResult> RankCandidates(
            List<JobSeekerProfile> candidates,
            Vacancy vacancy);

    }
}
