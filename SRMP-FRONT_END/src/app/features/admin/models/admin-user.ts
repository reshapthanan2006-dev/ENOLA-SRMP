import { UserRole } from '../../../core/models/auth-response';

export interface AdminUser {
  userId: number;
  fullName: string;
  email: string;
  role: UserRole;
  isActive: boolean;
  createdAt: string;
}