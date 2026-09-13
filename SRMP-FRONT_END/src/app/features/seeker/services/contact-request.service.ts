import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import {
  ContactRequest,
  ContactRequestStatus,
  CreateContactRequest
} from '../models/contact-request.model';

@Injectable({
  providedIn: 'root'
})
export class ContactRequestService {

  private apiUrl =
    `${environment.apiUrl}/ContactRequest`;

  constructor(
    private http: HttpClient
  ) { }

  createContactRequest(
    jobSeekerId: number,
    applicationId: number
  ): Observable<ContactRequest> {

    const request: CreateContactRequest = {
      jobSeekerId,
      applicationId
    };

    return this.http.post<ContactRequest>(
      this.apiUrl,
      request
    );
  }

  getMyRequests(): Observable<ContactRequest[]> {

    return this.http.get<ContactRequest[]>(
      `${this.apiUrl}/my`
    );
  }

  getSentRequests(): Observable<ContactRequest[]> {

    return this.http.get<ContactRequest[]>(
      `${this.apiUrl}/sent`
    );
  }

  respondToRequest(
    contactRequestId: number,
    status: ContactRequestStatus
  ): Observable<ContactRequest> {

    const params = new HttpParams()
      .set('status', status);

    return this.http.put<ContactRequest>(
      `${this.apiUrl}/${contactRequestId}/respond`,
      null,
      { params }
    );
  }

  acceptRequest(
    contactRequestId: number
  ): Observable<ContactRequest> {

    return this.respondToRequest(
      contactRequestId,
      'Accepted'
    );
  }

  declineRequest(
    contactRequestId: number
  ): Observable<ContactRequest> {

    return this.respondToRequest(
      contactRequestId,
      'Declined'
    );
  }
}