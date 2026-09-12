export interface SeekerProfile {
  id: number;
  userId: number;
  skills: string[];
  experienceYears: number;
  education: string;
  location: string;
}

export interface SeekerProfileRequest {
  skills: string[];
  experienceYears: number;
  education: string;
  location: string;
}
