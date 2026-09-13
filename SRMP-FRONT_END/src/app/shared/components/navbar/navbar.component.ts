import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';

import {
  Router,
  RouterLink,
  RouterLinkActive
} from '@angular/router';

import { AuthService } from '../../../core/auth/auth.service';
import { AuthUser } from '../../../core/models/auth-user';

import {
  NotificationStateService
} from '../../../core/services/notification.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    RouterLink,
    RouterLinkActive,
    AsyncPipe
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {

  readonly authService =
    inject(AuthService);

  private readonly router =
    inject(Router);

  private readonly notificationStateService =
    inject(NotificationStateService);

  readonly unreadCount$ =
    this.notificationStateService.unreadCount$;

  user: AuthUser | null =
    this.authService.getCurrentUser();

  isMenuOpen = false;

  toggleMenu(): void {
    this.isMenuOpen = !this.isMenuOpen;
  }

  closeMenu(): void {
    this.isMenuOpen = false;
  }

  logout(): void {

    this.notificationStateService
      .resetUnreadCount();

    this.authService.logout();

    this.isMenuOpen = false;

    this.router.navigate(['/login']);
  }
}