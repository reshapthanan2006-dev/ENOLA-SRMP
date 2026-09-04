using SRMP.Interfaces.Services;
using SRMP.Models;

namespace SRMP.Services
{
    public class MatchingService : IMatchingService
    {
        public MatchResult CalculateMatch(
        JobSeekerProfile profile,
        Vacancy vacancy)
        {
            profile.Skills ??= new List<string>();

            vacancy.RequiredSkills ??= new List<string>();

            var missingSkills = GetMissingSkills(profile, vacancy);

            var matchedSkills = vacancy.RequiredSkills
                .Count(skill =>
                    profile.Skills.Contains(
                        skill,
                        StringComparer.OrdinalIgnoreCase));

            var totalSkills = vacancy.RequiredSkills.Count;

            double skillScore = totalSkills == 0
                ? 0
                : (double)matchedSkills / totalSkills * 100;

            double experienceScore;

            if (vacancy.RequiredExperienceYears == 0)
            {
                experienceScore = 100;
            }
            else
            {
                experienceScore = Math.Min(
                    (double)profile.ExperienceYears /
                    vacancy.RequiredExperienceYears * 100,
                    100);
            }

            double matchScore = (skillScore + experienceScore) / 2;

            return new MatchResult
            {
                CandidateId = profile.Id,
                VacancyId = vacancy.Id,
                MatchScore = Math.Round(matchScore, 2),
                MissingSkills = missingSkills
            };
        }

        public List<string> GetMissingSkills(
            JobSeekerProfile profile,
            Vacancy vacancy)
        {
            profile.Skills ??= new List<string>();

            vacancy.RequiredSkills ??= new List<string>();

            var missingSkills = vacancy.RequiredSkills
                .Where(requiredSkill =>
                    !profile.Skills.Contains(
                        requiredSkill,
                        StringComparer.OrdinalIgnoreCase))
                .ToList();

            return missingSkills;
        }

        public List<MatchResult> RankCandidates(
            List<JobSeekerProfile> candidates,
            Vacancy vacancy)
        {
            var results = candidates
            .Select(candidate => CalculateMatch(candidate, vacancy))
            .OrderByDescending(result => result.MatchScore)
            .ToList();

            return results;
        }
    }
}