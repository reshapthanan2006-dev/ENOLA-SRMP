import { HttpErrorResponse } from '@angular/common/http';
import { Component, DestroyRef, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {
  AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators
} from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { ResetPasswordRequest } from '../../../../core/models/reset-password-request';

function passwordsMatch(control: AbstractControl): ValidationErrors | null {
  return control.get('newPassword')?.value === control.get('confirmPassword')?.value
    ? null
    : { passwordMismatch: true };
}

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly route = inject(ActivatedRoute);
  private readonly destroyRef = inject(DestroyRef);
  private readonly token = this.route.snapshot.queryParamMap.get('token');

  readonly hasToken = !!this.token;
  isSubmitting = false;
  successMessage = '';
  errorMessage = this.hasToken
    ? ''
    : 'The password reset link is missing its token. Please request a new link.';

  resetPasswordForm = this.formBuilder.nonNullable.group({
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', [Validators.required, Validators.minLength(6)]]
  }, { validators: passwordsMatch });

  onSubmit(): void {
    if (!this.token || this.isSubmitting || this.successMessage) {
      return;
    }

    this.errorMessage = '';

    if (this.resetPasswordForm.invalid) {
      this.resetPasswordForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    const formValue = this.resetPasswordForm.getRawValue();
    const request: ResetPasswordRequest = {
      token: this.token,
      newPassword: formValue.newPassword,
      confirmPassword: formValue.confirmPassword
    };

    this.authService.resetPassword(request)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (response) => {
          this.isSubmitting = false;
          this.successMessage = response.message;
          this.resetPasswordForm.reset();
        },
        error: (error: HttpErrorResponse) => {
          this.isSubmitting = false;
          if (error.status === 400) {
            this.errorMessage = error.error?.message ??
              'Unable to reset your password. Check your passwords or request a new reset link.';
          } else {
            this.errorMessage = error.status === 0
              ? 'Unable to reach the server. Please try again.'
              : 'Unable to reset your password. Please try again later.';
          }
        }
      });
  }
}
