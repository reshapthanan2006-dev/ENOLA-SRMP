import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { ApplicantProfile } from '../models/applicant-profile.model';

@Injectable({
  providedIn: 'root'
})
export class ApplicantProfileService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getApplicantProfile(
    jobVacancyId: number,
    jobSeekerId: number
  ): Observable<ApplicantProfile> {

    return this.http.get<ApplicantProfile>(
      `${this.apiUrl}/employer/applicants/vacancy/${jobVacancyId}/jobseeker/${jobSeekerId}/profile`
    );
  }
}