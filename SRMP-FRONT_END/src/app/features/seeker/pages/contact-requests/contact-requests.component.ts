import {
  DatePipe
} from '@angular/common';

import {
  HttpErrorResponse
} from '@angular/common/http';

import {
  Component,
  DestroyRef,
  OnInit,
  inject
} from '@angular/core';

import {
  RouterLink
} from '@angular/router';

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

  imports: [
    DatePipe,
    RouterLink
  ],

  templateUrl:
    './contact-requests.component.html',

  styleUrl:
    './contact-requests.component.css'
})
export class ContactRequestsComponent
  implements OnInit {

  private readonly contactRequestService =
    inject(ContactRequestService);

  private readonly destroyRef =
    inject(DestroyRef);


  contactRequests:
    ContactRequest[] = [];


  isLoading = false;

  errorMessage = '';


  respondingRequestId:
    number | null = null;


  responseMessage = '';

  responseErrorMessage = '';


  ngOnInit(): void {

    this.loadContactRequests();

  }


  // ==========================================
  // LOAD CONTACT REQUESTS
  // ==========================================

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

        next: (
          data: ContactRequest[]
        ) => {

          this.contactRequests =
            data;

          this.isLoading = false;

        },


        error: (
          error: HttpErrorResponse
        ) => {

          this.isLoading = false;

          if (
            error.status === 401
          ) {

            this.errorMessage =
              'Please sign in to view your contact requests.';

            return;

          }


          if (
            error.status === 403
          ) {

            this.errorMessage =
              'Your account is not allowed to view contact requests.';

            return;

          }


          this.errorMessage =
            'Unable to load your contact requests. Please try again.';

        }

      });

  }


  // ==========================================
  // ACCEPT REQUEST
  // ==========================================

  acceptRequest(
    contactRequestId: number
  ): void {

    const confirmed =
      window.confirm(
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


  // ==========================================
  // DECLINE REQUEST
  // ==========================================

  declineRequest(
    contactRequestId: number
  ): void {

    const confirmed =
      window.confirm(
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


  // ==========================================
  // RESPOND TO REQUEST
  // ==========================================

  private respondToRequest(
    contactRequestId: number,
    status: ContactRequestStatus
  ): void {

    if (
      this.respondingRequestId !== null
    ) {

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

        next: (
          updatedRequest:
            ContactRequest
        ) => {

          this.contactRequests =
            this.contactRequests.map(
              request =>
                request.contactRequestId ===
                updatedRequest.contactRequestId

                  ? updatedRequest

                  : request
            );


          this.respondingRequestId =
            null;


          this.responseMessage =
            status === 'Accepted'

              ? 'Contact request accepted successfully.'

              : 'Contact request declined.';

        },


        error: (
          error: HttpErrorResponse
        ) => {

          this.respondingRequestId =
            null;


          if (
            error.status === 400
          ) {

            this.responseErrorMessage =
              error.error?.message ??
              'This contact request has already been responded to.';

            return;

          }


          if (
            error.status === 401
          ) {

            this.responseErrorMessage =
              'Please sign in to respond to this contact request.';

            return;

          }


          if (
            error.status === 403
          ) {

            this.responseErrorMessage =
              'You are not allowed to respond to this contact request.';

            return;

          }


          if (
            error.status === 404
          ) {

            this.responseErrorMessage =
              'Contact request not found.';

            return;

          }


          this.responseErrorMessage =
            'Unable to update the contact request. Please try again.';

        }

      });

  }


  // ==========================================
  // HELPERS
  // ==========================================

  isPending(
    request: ContactRequest
  ): boolean {

    return (
      request.status === 'Pending'
    );

  }


  isResponding(
    contactRequestId: number
  ): boolean {

    return (
      this.respondingRequestId ===
      contactRequestId
    );

  }

}