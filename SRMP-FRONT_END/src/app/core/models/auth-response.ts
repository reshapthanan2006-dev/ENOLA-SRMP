export type UserRole =
  | 'JobSeeker'
  | 'Employer'
  | 'Administrator';

export interface AuthResponse {
  userId: number;
  fullName: string;
  email: string;
  role: UserRole;
  token: string;
}