import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import { environment } from '../../../environments/environment';
import { LoginRequest } from '../models/login-request';
import { RegisterRequest } from '../models/register-request';
import { AuthResponse } from '../models/auth-response';
import { AuthUser } from '../models/auth-user';
import { StorageService } from '../services/storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly http = inject(HttpClient);

  private readonly storageService =
    inject(StorageService);

  private readonly apiUrl =
    `${environment.apiUrl}/Auth`;

  login(
    request: LoginRequest
  ): Observable<AuthResponse> {

    return this.http
      .post<AuthResponse>(
        `${this.apiUrl}/login`,
        request
      )
      .pipe(
        tap(response => {
          this.storageService.saveAuth(response);
        })
      );
  }

  register(
    request: RegisterRequest
  ): Observable<AuthResponse> {

    return this.http
      .post<AuthResponse>(
        `${this.apiUrl}/register`,
        request
      )
      .pipe(
        tap(response => {
          this.storageService.saveAuth(response);
        })
      );
  }

  logout(): void {
    this.storageService.clearAuth();
  }

  isLoggedIn(): boolean {
    return this.storageService.isLoggedIn();
  }

  getCurrentUser(): AuthUser | null {
    return this.storageService.getUser();
  }

  getToken(): string | null {
    return this.storageService.getToken();
  }
}