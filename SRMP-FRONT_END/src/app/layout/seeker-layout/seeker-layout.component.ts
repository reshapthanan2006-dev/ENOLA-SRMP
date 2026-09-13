import {
  Component,
  DestroyRef,
  OnInit,
  inject
} from '@angular/core';

import {
  Router,
  RouterLink,
  RouterLinkActive,
  RouterOutlet
} from '@angular/router';

import { AsyncPipe } from '@angular/common';

import {
  takeUntilDestroyed
} from '@angular/core/rxjs-interop';

import {
  AuthService
} from '../../core/auth/auth.service';

import {
  NotificationStateService
} from '../../core/services/notification.service';

import {
  NotificationService
} from '../../features/notifications/services/notification.service';

@Component({
  selector: 'app-seeker-layout',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    AsyncPipe
  ],
  templateUrl: './seeker-layout.component.html',
  styleUrl: './seeker-layout.component.css'
})
export class SeekerLayoutComponent implements OnInit {

  private readonly authService =
    inject(AuthService);

  private readonly router =
    inject(Router);

  private readonly notificationService =
    inject(NotificationService);

  private readonly notificationStateService =
    inject(NotificationStateService);

  private readonly destroyRef =
    inject(DestroyRef);

  readonly unreadCount$ =
    this.notificationStateService.unreadCount$;

  ngOnInit(): void {
    this.loadUnreadNotificationCount();
  }

  private loadUnreadNotificationCount(): void {

    this.notificationService
      .getMyNotifications()
      .pipe(
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe({
        next: (notifications) => {

          const unreadCount =
            notifications.filter(
              (notification) =>
                !notification.isRead
            ).length;

          this.notificationStateService
            .setUnreadCount(unreadCount);
        },

        error: () => {
          this.notificationStateService
            .resetUnreadCount();
        }
      });
  }

  logout(): void {

    this.notificationStateService
      .resetUnreadCount();

    this.authService.logout();

    this.router.navigate(['/login']);
  }
}