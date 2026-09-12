import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

import { SeekerCv } from '../../models/cv.model';
import { SeekerProfile } from '../../models/seeker-profile.model';
import { CvService } from '../../services/cv.service';
import { SeekerProfileService } from '../../services/seeker-profile.service';

@Component({
  selector: 'app-seeker-dashboard',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  profile: SeekerProfile | null = null;
  cv: SeekerCv | null = null;

  profileLoading = true;
  cvLoading = true;
  errorMessage = '';

  constructor(
    private profileService: SeekerProfileService,
    private cvService: CvService
  ) { }

  ngOnInit(): void {
    this.loadProfile();
    this.loadCv();
  }

  get isLoading(): boolean {
    return this.profileLoading || this.cvLoading;
  }

  get skillsCount(): number {
    return this.profile?.skills.length ?? 0;
  }

  formatFileSize(bytes: number): string {
    if (bytes < 1024 * 1024) {
      return `${(bytes / 1024).toFixed(1)} KB`;
    }

    return `${(bytes / (1024 * 1024)).toFixed(2)} MB`;
  }

  private loadProfile(): void {
    this.profileService.getProfile().subscribe({
      next: (profile) => {
        this.profile = profile;
        this.profileLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.profileLoading = false;

        if (error.status !== 404) {
          this.errorMessage = 'Some dashboard information could not be loaded.';
        }
      }
    });
  }

  private loadCv(): void {
    this.cvService.getCv().subscribe({
      next: (cv) => {
        this.cv = cv;
        this.cvLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.cvLoading = false;

        if (error.status !== 404) {
          this.errorMessage = 'Some dashboard information could not be loaded.';
        }
      }
    });
  }
}
