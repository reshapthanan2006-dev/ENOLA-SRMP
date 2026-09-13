import {
  Component,
  DestroyRef,
  OnInit,
  inject
} from '@angular/core';

import {
  Router
} from '@angular/router';

import {
  HttpErrorResponse
} from '@angular/common/http';

import {
  takeUntilDestroyed
} from '@angular/core/rxjs-interop';

import {
  Vacancy
} from '../../models/vacancy.model';

import {
  VacancyService
} from '../../services/vacancy.service';

@Component({
  selector: 'app-vacancies',
  standalone: true,
  imports: [],
  templateUrl: './vacancies.component.html',
  styleUrl: './vacancies.component.css'
})
export class VacanciesComponent implements OnInit {

  private readonly router =
    inject(Router);

  private readonly vacancyService =
    inject(VacancyService);

  private readonly destroyRef =
    inject(DestroyRef);

  vacancies: Vacancy[] = [];

  isLoading = false;

  closingVacancyId: number | null = null;

  errorMessage = '';

  ngOnInit(): void {
    this.loadVacancies();
  }

  loadVacancies(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.vacancyService
      .getVacancies()
      .pipe(
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe({

        next: vacancies => {

          this.vacancies = vacancies;

          this.isLoading = false;
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.vacancies = [];

          this.isLoading = false;

          this.errorMessage =
            this.getErrorMessage(error);

          window.alert(
            this.errorMessage
          );
        }

      });
  }

  openCreateVacancy(): void {

    this.router.navigate([
      '/employer/vacancies/new'
    ]);
  }

  closeVacancy(
    vacancyId: number
  ): void {

    const confirmed =
      window.confirm(
        'Are you sure you want to close this vacancy?'
      );

    if (!confirmed) {
      return;
    }

    this.closingVacancyId =
      vacancyId;

    this.vacancyService
      .closeVacancy(vacancyId)
      .pipe(
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe({

        next: () => {

          this.closingVacancyId = null;

          this.loadVacancies();
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.closingVacancyId = null;

          window.alert(
            this.getErrorMessage(error)
          );
        }

      });
  }

  openEditVacancy(
    vacancyId: number
  ): void {

    this.router.navigate([
      '/employer/vacancies/edit',
      vacancyId
    ]);
  }

  private getErrorMessage(
    error: HttpErrorResponse
  ): string {

    if (error.status === 401) {
      return 'Please sign in as an employer.';
    }

    if (error.status === 403) {
      return 'You are not allowed to manage this vacancy.';
    }

    if (error.status === 404) {
      return 'Vacancy not found.';
    }

    if (
      typeof error.error === 'string' &&
      error.error.trim()
    ) {
      return error.error;
    }

    if (
      error.error?.message
    ) {
      return error.error.message;
    }

    return 'Unable to load vacancies. Please try again.';
  }
}