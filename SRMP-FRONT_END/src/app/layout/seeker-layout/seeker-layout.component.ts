import {
  Component,
  DestroyRef,
  OnInit,
  inject
} from '@angular/core';

import { RouterOutlet } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { NavbarComponent } from '../../shared/components/navbar/navbar.component';

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
    NavbarComponent
  ],
  templateUrl: './seeker-layout.component.html',
  styleUrl: './seeker-layout.component.css'
})
export class SeekerLayoutComponent implements OnInit {

  private readonly notificationService =
    inject(NotificationService);

  private readonly notificationStateService =
    inject(NotificationStateService);

  private readonly destroyRef =
    inject(DestroyRef);

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
              notification => !notification.isRead
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
}