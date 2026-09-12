import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AdminDashboard } from '../models/admin-dashboard';
import { AdminUser } from '../models/admin-user';
import { UpdateUserStatus } from '../models/update-user-status';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    'https://localhost:7184/api/Admin';

  getDashboard(): Observable<AdminDashboard> {
    return this.http.get<AdminDashboard>(
      `${this.apiUrl}/dashboard`
    );
  }

  getUsers(): Observable<AdminUser[]> {
    return this.http.get<AdminUser[]>(
      `${this.apiUrl}/users`
    );
  }

  updateUserStatus(
    userId: number,
    isActive: boolean
  ): Observable<AdminUser> {
    const request: UpdateUserStatus = {
      isActive
    };

    return this.http.put<AdminUser>(
      `${this.apiUrl}/users/${userId}/status`,
      request
    );
  }
}