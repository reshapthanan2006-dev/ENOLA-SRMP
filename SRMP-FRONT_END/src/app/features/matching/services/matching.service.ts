import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { MatchResult } from '../models/match-result.model';
import { MatchingJob } from '../models/matching-job.model';

@Injectable({
  providedIn: 'root'
})
export class MatchingService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getRankedApplicants(
    jobVacancyId: number,
    employerId: number
  ): Observable<MatchResult[]> {

    return this.http.get<MatchResult[]>(
      `${this.apiUrl}/Application/vacancy/${jobVacancyId}/ranked?employerId=${employerId}`
    );
  }

  getJobSeekerJobDetail(
    jobVacancyId: number
  ): Observable<MatchingJob> {

    return this.http.get<MatchingJob>(
      `${this.apiUrl}/JobVacancy/${jobVacancyId}/jobseeker-detail`
    );
  }

}