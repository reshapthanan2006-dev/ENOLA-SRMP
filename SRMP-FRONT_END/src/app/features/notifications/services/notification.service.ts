import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';

import {
  AppNotification,
  MarkNotificationReadResponse
} from '../models/notification.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private apiUrl =
    `${environment.apiUrl}/Notification`;

  constructor(
    private http: HttpClient
  ) { }

  getMyNotifications(): Observable<AppNotification[]> {

    return this.http.get<AppNotification[]>(
      `${this.apiUrl}/my`
    );
  }

  markAsRead(
    notificationId: number
  ): Observable<MarkNotificationReadResponse> {

    return this.http.put<MarkNotificationReadResponse>(
      `${this.apiUrl}/${notificationId}/read`,
      null
    );
  }
}