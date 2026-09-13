import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  DestroyRef,
  OnInit
} from '@angular/core';

import {
  takeUntilDestroyed
} from '@angular/core/rxjs-interop';

import {
  AppNotification
} from '../../models/notification.model';

import {
  NotificationService
} from '../../services/notification.service';

import {
  NotificationStateService
} from '../../../../core/services/notification.service';

import {
  NotificationItemComponent
} from '../../components/notification-item/notification-item.component';

@Component({
  selector: 'app-notification-list',
  standalone: true,
  imports: [
    NotificationItemComponent
  ],
  templateUrl: './notification-list.component.html',
  styleUrl: './notification-list.component.css'
})
export class NotificationListComponent implements OnInit {

  notifications: AppNotification[] = [];

  isLoading = false;
  errorMessage = '';

  markingNotificationId: number | null = null;

  actionMessage = '';
  actionErrorMessage = '';

  unreadCount = 0;

  constructor(
    private notificationService: NotificationService,
    private notificationStateService: NotificationStateService,
    private destroyRef: DestroyRef
  ) { }

  ngOnInit(): void {
    this.loadNotifications();
  }

  loadNotifications(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.actionMessage = '';
    this.actionErrorMessage = '';

    this.notificationService
      .getMyNotifications()
      .pipe(
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe({

        next: (data) => {

          this.notifications = data;

          this.updateUnreadCount();

          this.isLoading = false;
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.isLoading = false;

          this.notifications = [];

          this.notificationStateService
            .resetUnreadCount();

          this.unreadCount = 0;

          if (error.status === 401) {

            this.errorMessage =
              'Please sign in to view your notifications.';

            return;
          }

          if (error.status === 403) {

            this.errorMessage =
              'You are not allowed to view job seeker notifications.';

            return;
          }

          this.errorMessage =
            'Unable to load notifications.';
        }

      });
  }

  markAsRead(
    notificationId: number
  ): void {

    if (
      this.markingNotificationId !== null
    ) {
      return;
    }

    const notification =
      this.notifications.find(
        (item) =>
          item.notificationId === notificationId
      );

    if (
      !notification ||
      notification.isRead
    ) {
      return;
    }

    this.markingNotificationId =
      notificationId;

    this.actionMessage = '';
    this.actionErrorMessage = '';

    this.notificationService
      .markAsRead(notificationId)
      .pipe(
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe({

        next: () => {

          this.notifications =
            this.notifications.map(
              (item) =>
                item.notificationId ===
                notificationId
                  ? {
                      ...item,
                      isRead: true
                    }
                  : item
            );

          this.markingNotificationId = null;

          this.updateUnreadCount();

          this.actionMessage =
            'Notification marked as read.';
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.markingNotificationId = null;

          if (error.status === 401) {

            this.actionErrorMessage =
              'Please sign in to update this notification.';

            return;
          }

          if (error.status === 403) {

            this.actionErrorMessage =
              'You are not allowed to update this notification.';

            return;
          }

          if (error.status === 404) {

            this.actionErrorMessage =
              'Notification not found or access denied.';

            return;
          }

          this.actionErrorMessage =
            'Unable to mark notification as read.';
        }

      });
  }

  isMarkingRead(
    notificationId: number
  ): boolean {

    return this.markingNotificationId ===
      notificationId;
  }

  private updateUnreadCount(): void {

    this.unreadCount =
      this.notifications.filter(
        (notification) =>
          !notification.isRead
      ).length;

    this.notificationStateService
      .setUnreadCount(
        this.unreadCount
      );
  }
}