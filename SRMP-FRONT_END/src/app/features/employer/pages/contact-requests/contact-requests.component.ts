import { DatePipe } from '@angular/common';
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
  ContactRequest
} from '../../../seeker/models/contact-request.model';

import {
  ContactRequestService
} from '../../../seeker/services/contact-request.service';

@Component({
  selector: 'app-employer-contact-requests',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './contact-requests.component.html',
  styleUrl: './contact-requests.component.css'
})
export class ContactRequestsComponent implements OnInit {

  contactRequests: ContactRequest[] = [];

  isLoading = false;
  errorMessage = '';

  constructor(
    private contactRequestService: ContactRequestService,
    private destroyRef: DestroyRef
  ) { }

  ngOnInit(): void {
    this.loadContactRequests();
  }

  loadContactRequests(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.contactRequestService
      .getSentRequests()
      .pipe(
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe({

        next: (data) => {

          this.contactRequests = data;

          this.isLoading = false;
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.isLoading = false;

          if (error.status === 401) {

            this.errorMessage =
              'Please sign in as an employer to view sent contact requests.';

            return;
          }

          if (error.status === 403) {

            this.errorMessage =
              'You are not allowed to view employer contact requests.';

            return;
          }

          this.errorMessage =
            'Unable to load sent contact requests.';
        }

      });
  }

  isPending(
    request: ContactRequest
  ): boolean {

    return request.status === 'Pending';
  }

  isAccepted(
    request: ContactRequest
  ): boolean {

    return request.status === 'Accepted';
  }

  isDeclined(
    request: ContactRequest
  ): boolean {

    return request.status === 'Declined';
  }
}