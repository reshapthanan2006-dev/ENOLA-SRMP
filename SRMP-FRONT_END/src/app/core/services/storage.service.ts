import { Injectable } from '@angular/core';
import { AuthResponse } from '../models/auth-response';
import { AuthUser } from '../models/auth-user';

@Injectable({
  providedIn: 'root'
})
export class StorageService {

  private readonly tokenKey = 'srmp_token';
  private readonly userKey = 'srmp_user';

  saveAuth(response: AuthResponse): void {

    const user: AuthUser = {
      userId: response.userId,
      fullName: response.fullName,
      email: response.email,
      role: response.role
    };

    localStorage.setItem(
      this.tokenKey,
      response.token
    );

    localStorage.setItem(
      this.userKey,
      JSON.stringify(user)
    );
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getUser(): AuthUser | null {

    const storedUser =
      localStorage.getItem(this.userKey);

    if (!storedUser) {
      return null;
    }

    try {
      return JSON.parse(storedUser) as AuthUser;
    } catch {
      this.clearAuth();
      return null;
    }
  }

  isLoggedIn(): boolean {
    return this.getToken() !== null;
  }

  clearAuth(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
  }
}