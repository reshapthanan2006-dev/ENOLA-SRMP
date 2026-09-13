export interface Vacancy {
  jobVacancyId: number;
  title: string;
  description: string;
  requiredSkills: string;
  requiredExperience: number;
  requiredEducation: string;
  location: string;
  employerId: number;
  isOpen: boolean;
  createdAt: string;
}

export interface VacancyRequest {
  title: string;
  description: string;
  requiredSkills: string;
  requiredExperience: number;
  requiredEducation: string;
  location: string;
}