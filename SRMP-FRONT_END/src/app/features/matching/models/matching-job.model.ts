export interface MatchingJob {
  jobVacancyId: number;
  title: string;
  description: string;
  requiredSkills: string;
  requiredExperience: number;
  location: string;
  isOpen: boolean;
  matchScore: number;
  missingSkills: string[];
}