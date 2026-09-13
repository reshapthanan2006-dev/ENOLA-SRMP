import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import {
  Application,
  CreateApplication,
  UpdateApplicationStatus
} from '../models/application.model';

@Injectable({
  providedIn: 'root'
})
export class ApplicationService {

  private apiUrl = `${environment.apiUrl}/Application`;

  constructor(private http: HttpClient) { }

  applyForJob(jobVacancyId: number): Observable<Application> {
    const request: CreateApplication = {
      jobVacancyId: jobVacancyId
    };

    return this.http.post<Application>(
      this.apiUrl,
      request
    );
  }

  getMyApplications(): Observable<Application[]> {
    return this.http.get<Application[]>(
      `${this.apiUrl}/my`
    );
  }

  getApplicationsByVacancy(
    jobVacancyId: number
  ): Observable<Application[]> {
    return this.http.get<Application[]>(
      `${this.apiUrl}/vacancy/${jobVacancyId}`
    );
  }

  updateApplicationStatus(
    applicationId: number,
    status: string
  ): Observable<Application> {

    const request: UpdateApplicationStatus = {
      status: status
    };

    return this.http.put<Application>(
      `${this.apiUrl}/${applicationId}/status`,
      request
    );
  }
}