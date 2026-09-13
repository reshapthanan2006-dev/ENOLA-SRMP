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
  ContactRequest,
  ContactRequestStatus
} from '../../models/contact-request.model';

import {
  ContactRequestService
} from '../../services/contact-request.service';

@Component({
  selector: 'app-contact-requests',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './contact-requests.component.html',
  styleUrl: './contact-requests.component.css'
})
export class ContactRequestsComponent implements OnInit {

  contactRequests: ContactRequest[] = [];

  isLoading = false;
  errorMessage = '';

  respondingRequestId: number | null = null;

  responseMessage = '';
  responseErrorMessage = '';

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

    this.responseMessage = '';
    this.responseErrorMessage = '';

    this.contactRequestService
      .getMyRequests()
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

        error: () => {

          this.errorMessage =
            'Unable to load your contact requests.';

          this.isLoading = false;
        }

      });
  }

  acceptRequest(
    contactRequestId: number
  ): void {

    const confirmed = window.confirm(
      'Are you sure you want to accept this contact request?'
    );

    if (!confirmed) {
      return;
    }

    this.respondToRequest(
      contactRequestId,
      'Accepted'
    );
  }

  declineRequest(
    contactRequestId: number
  ): void {

    const confirmed = window.confirm(
      'Are you sure you want to decline this contact request?'
    );

    if (!confirmed) {
      return;
    }

    this.respondToRequest(
      contactRequestId,
      'Declined'
    );
  }

  private respondToRequest(
    contactRequestId: number,
    status: ContactRequestStatus
  ): void {

    if (this.respondingRequestId !== null) {
      return;
    }

    this.respondingRequestId =
      contactRequestId;

    this.responseMessage = '';
    this.responseErrorMessage = '';

    this.contactRequestService
      .respondToRequest(
        contactRequestId,
        status
      )
      .pipe(
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe({

        next: (updatedRequest) => {

          this.contactRequests =
            this.contactRequests.map(
              (request) =>
                request.contactRequestId ===
                updatedRequest.contactRequestId
                  ? updatedRequest
                  : request
            );

          this.respondingRequestId = null;

          this.responseMessage =
            status === 'Accepted'
              ? 'Contact request accepted.'
              : 'Contact request declined.';
        },

        error: (
          error: HttpErrorResponse
        ) => {

          this.respondingRequestId = null;

          if (error.status === 400) {

            this.responseErrorMessage =
              'This contact request has already been responded to.';

            return;
          }

          if (error.status === 401) {

            this.responseErrorMessage =
              'Please sign in to respond to this contact request.';

            return;
          }

          if (error.status === 404) {

            this.responseErrorMessage =
              'Contact request not found.';

            return;
          }

          this.responseErrorMessage =
            'Unable to update the contact request. Please try again.';
        }

      });
  }

  isPending(
    request: ContactRequest
  ): boolean {

    return request.status === 'Pending';
  }

  isResponding(
    contactRequestId: number
  ): boolean {

    return this.respondingRequestId ===
      contactRequestId;
  }
}