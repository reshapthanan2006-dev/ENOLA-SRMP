import { CommonModule } from '@angular/common';
import {
  Component,
  DestroyRef,
  OnInit
} from '@angular/core';
import {
  takeUntilDestroyed
} from '@angular/core/rxjs-interop';

import {
  Application as JobApplication
} from '../../models/application.model';

import {
  ApplicationService
} from '../../services/application.service';

@Component({
  selector: 'app-applications',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './applications.component.html',
  styleUrl: './applications.component.css'
})
export class ApplicationsComponent implements OnInit {

  applications: JobApplication[] = [];

  isLoading = false;
  errorMessage = '';

  constructor(
    private applicationService: ApplicationService,
    private destroyRef: DestroyRef
  ) { }

  ngOnInit(): void {
    this.loadApplications();
  }

  loadApplications(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.applicationService
      .getMyApplications()
      .pipe(
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe({

        next: (data) => {

          this.applications = data;

          this.isLoading = false;
        },

        error: () => {

          this.errorMessage =
            'Unable to load your applications.';

          this.isLoading = false;
        }

      });
  }
}