import { UserRole } from './auth-response';

export interface AuthUser {
  userId: number;
  fullName: string;
  email: string;
  role: UserRole;
}