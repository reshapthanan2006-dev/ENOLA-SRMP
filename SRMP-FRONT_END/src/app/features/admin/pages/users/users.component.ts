import { Component, OnInit, inject } from '@angular/core';
import { Router} from '@angular/router';
import { NavbarComponent } from '../../../../shared/components/navbar/navbar.component';
import { AdminService } from '../../services/admin.service';
import { AdminUser } from '../../models/admin-user';
import { AuthService } from '../../../../core/auth/auth.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    NavbarComponent,DatePipe
  ],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent implements OnInit {
  private readonly adminService =
    inject(AdminService);

  private readonly authService =
    inject(AuthService);

  private readonly router =
    inject(Router);

  users: AdminUser[] = [];

  isLoading = true;
  errorMessage = '';

  actionUserId: number | null = null;

  currentUserId: number | null =
  this.authService.getCurrentUser()?.userId ?? null;

  ngOnInit(): void {
    this.loadUsers();
  }

  isCurrentUser(user: AdminUser): boolean {
  return user.userId === this.currentUserId;
}

  loadUsers(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.adminService.getUsers().subscribe({
      next: users => {
        this.users = users;
        this.isLoading = false;
      },

      error: error => {
        this.isLoading = false;

        this.errorMessage =
          error?.error?.message ??
          'Unable to load users.';
      }
    });
  }

  toggleUserStatus(user: AdminUser): void {
    this.actionUserId = user.userId;
    this.errorMessage = '';

    this.adminService
      .updateUserStatus(
        user.userId,
        !user.isActive
      )
      .subscribe({
        next: updatedUser => {
          this.users = this.users.map(item =>
            item.userId === updatedUser.userId
              ? updatedUser
              : item
          );

          this.actionUserId = null;
        },

        error: error => {
          this.actionUserId = null;

          this.errorMessage =
            error?.error?.message ??
            'Unable to update user status.';
        }
      });
  }

  logout(): void {
    this.authService.logout();

    this.router.navigate([
      '/login'
    ]);
  }
}