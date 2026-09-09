export interface MatchResult {
  candidateId: number;
  vacancyId: number;
  matchScore: number;
  skillsScore: number;
  experienceScore: number;
  educationScore: number;
  locationScore: number;
  missingSkills: string[];
}