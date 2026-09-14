import { HttpErrorResponse } from '@angular/common/http';
import { Component, DestroyRef, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { ForgotPasswordRequest } from '../../../../core/models/forgot-password-request';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly destroyRef = inject(DestroyRef);

  isSubmitting = false;
  successMessage = '';
  errorMessage = '';

  forgotPasswordForm = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email]]
  });

  onSubmit(): void {
    if (this.isSubmitting) {
      return;
    }

    this.successMessage = '';
    this.errorMessage = '';

    if (this.forgotPasswordForm.invalid) {
      this.forgotPasswordForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    const request: ForgotPasswordRequest = this.forgotPasswordForm.getRawValue();

    this.authService.forgotPassword(request)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (response) => {
          this.isSubmitting = false;
          this.successMessage = response.message;
        },
        error: (error: HttpErrorResponse) => {
          this.isSubmitting = false;
          this.errorMessage = error.status === 0
            ? 'Unable to reach the server. Please try again.'
            : 'Unable to request a reset link. Please check your email address and try again.';
        }
      });
  }
}
