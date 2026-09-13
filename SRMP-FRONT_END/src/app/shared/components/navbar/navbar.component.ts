import { Component, inject } from '@angular/core';
import {
  Router,
  RouterLink,
  RouterLinkActive
} from '@angular/router';

import { AuthService } from '../../../core/auth/auth.service';
import { AuthUser } from '../../../core/models/auth-user';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  user: AuthUser | null =
    this.authService.getCurrentUser();

  logout(): void {
    this.authService.logout();

    this.router.navigate([
      '/login'
    ]);
  }
}