export interface MatchResult {
  applicationId: number;
  jobSeekerId: number;
  jobVacancyId: number;
  status: string;
  appliedAt: string;
  matchScore: number;
  skillsScore: number;
  experienceScore: number;
  educationScore: number;
  locationScore: number;
  missingSkills: string[];
}