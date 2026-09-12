export enum RegistrationRole {
  JobSeeker = 0,
  Employer = 1
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  role: RegistrationRole;
}