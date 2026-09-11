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

            // Normalize candidate skills:
            // trim spaces + remove empty + remove duplicates
            var candidateSkills = profile.Skills
                .Where(skill => !string.IsNullOrWhiteSpace(skill))
                .Select(skill => skill.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // Normalize required skills:
            // trim spaces + remove empty + remove duplicates
            var requiredSkills = vacancy.RequiredSkills
                .Where(skill => !string.IsNullOrWhiteSpace(skill))
                .Select(skill => skill.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var missingSkills = GetMissingSkills(
                candidateSkills,
                requiredSkills);

            // -----------------------------
            // 1. Skills Score - 50%
            // -----------------------------
            var matchedSkills = requiredSkills
                .Count(requiredSkill =>
                    candidateSkills.Contains(
                        requiredSkill,
                        StringComparer.OrdinalIgnoreCase));

            var totalSkills = requiredSkills.Count;

            double skillScore = totalSkills == 0
                ? 0
                : (double)matchedSkills / totalSkills * 100;

            // -----------------------------
            // 2. Experience Score - 25%
            // -----------------------------
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

            // -----------------------------
            // 3. Education Score - 15%
            // Higher or equal qualification = 100
            // Lower qualification = 0
            // -----------------------------
            double educationScore = GetEducationScore(
                profile.Education,
                vacancy.RequiredEducation);

            // -----------------------------
            // 4. Location Score - 10%
            // Same city = 100
            // Same district = 50
            // Different district = 0
            // -----------------------------
            double locationScore = GetLocationScore(
                profile.Location,
                vacancy.Location);

            // -----------------------------
            // Final weighted score
            // -----------------------------
            double matchScore =
                (skillScore * 0.50) +
                (experienceScore * 0.25) +
                (educationScore * 0.15) +
                (locationScore * 0.10);

            return new MatchResult
            {
                CandidateId = profile.Id,
                VacancyId = vacancy.Id,

                MatchScore = Math.Round(
                    matchScore,
                    2,
                    MidpointRounding.AwayFromZero),

                SkillsScore = Math.Round(
                    skillScore,
                    2),

                ExperienceScore = Math.Round(
                    experienceScore,
                    2),

                EducationScore = Math.Round(
                    educationScore,
                    2),

                LocationScore = Math.Round(
                    locationScore,
                    2),

                MissingSkills = missingSkills
            };
        }

        // Public method used by interface / other services
        public List<string> GetMissingSkills(
            JobSeekerProfile profile,
            Vacancy vacancy)
        {
            profile.Skills ??= new List<string>();
            vacancy.RequiredSkills ??= new List<string>();

            var candidateSkills = profile.Skills
                .Where(skill => !string.IsNullOrWhiteSpace(skill))
                .Select(skill => skill.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var requiredSkills = vacancy.RequiredSkills
                .Where(skill => !string.IsNullOrWhiteSpace(skill))
                .Select(skill => skill.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            return GetMissingSkills(
                candidateSkills,
                requiredSkills);
        }

        // Internal helper for normalized skill lists
        private List<string> GetMissingSkills(
            List<string> candidateSkills,
            List<string> requiredSkills)
        {
            return requiredSkills
                .Where(requiredSkill =>
                    !candidateSkills.Contains(
                        requiredSkill,
                        StringComparer.OrdinalIgnoreCase))
                .ToList();
        }

        private double GetEducationScore(
            string candidateEducation,
            string requiredEducation)
        {
            if (string.IsNullOrWhiteSpace(requiredEducation))
            {
                return 0;
            }

            int candidateLevel =
                GetEducationLevel(candidateEducation);

            int requiredLevel =
                GetEducationLevel(requiredEducation);

            if (candidateLevel >= requiredLevel &&
                requiredLevel > 0)
            {
                return 100;
            }

            return 0;
        }

        private int GetEducationLevel(
            string education)
        {
            if (string.IsNullOrWhiteSpace(education))
            {
                return 0;
            }

            string value = education.ToLower();

            if (value.Contains("phd") ||
                value.Contains("doctorate") ||
                value.Contains("doctoral"))
            {
                return 4;
            }

            if (value.Contains("master") ||
                value.Contains("m.sc") ||
                value.Contains("msc") ||
                value.Contains("m.tech") ||
                value.Contains("mtech") ||
                value.Contains("mba"))
            {
                return 3;
            }

            if (value.Contains("bachelor") ||
                value.Contains("b.sc") ||
                value.Contains("bsc") ||
                value.Contains("b.tech") ||
                value.Contains("btech") ||
                value.Contains("b.e") ||
                value.Contains("be") ||
                value.Contains("degree"))
            {
                return 2;
            }

            if (value.Contains("diploma"))
            {
                return 1;
            }

            return 0;
        }

        private double GetLocationScore(
            string candidateLocation,
            string vacancyLocation)
        {
            if (string.IsNullOrWhiteSpace(candidateLocation) ||
                string.IsNullOrWhiteSpace(vacancyLocation))
            {
                return 0;
            }

            string candidate =
                candidateLocation.Trim().ToLower();

            string vacancy =
                vacancyLocation.Trim().ToLower();

            // Same city / exact location
            if (candidate == vacancy)
            {
                return 100;
            }

            // Location format expected:
            // City, District
            var candidateParts = candidate
                .Split(
                    ',',
                    StringSplitOptions.RemoveEmptyEntries);

            var vacancyParts = vacancy
                .Split(
                    ',',
                    StringSplitOptions.RemoveEmptyEntries);

            if (candidateParts.Length >= 2 &&
                vacancyParts.Length >= 2)
            {
                string candidateDistrict =
                    candidateParts[1].Trim();

                string vacancyDistrict =
                    vacancyParts[1].Trim();

                // Same district
                if (candidateDistrict ==
                    vacancyDistrict)
                {
                    return 50;
                }
            }

            // Different district
            return 0;
        }

        public List<MatchResult> RankCandidates(
            List<JobSeekerProfile> candidates,
            Vacancy vacancy)
        {
            var results = candidates
                .Select(candidate =>
                    CalculateMatch(candidate, vacancy))
                .OrderByDescending(
                    result => result.MatchScore)
                .ThenByDescending(
                    result => result.SkillsScore)
                .ThenByDescending(
                    result => result.ExperienceScore)
                .ThenByDescending(
                    result => result.EducationScore)
                .ThenByDescending(
                    result => result.LocationScore)
                .ToList();

            return results;
        }
    }
}