import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NavbarComponent } from '../../../../shared/components/navbar/navbar.component';
import { SkillRowComponent } from '../../components/skill-row/skill-row.component';
import {
  SeekerProfile,
  SeekerProfileRequest
} from '../../models/seeker-profile.model';
import { SeekerProfileService } from '../../services/seeker-profile.service';

@Component({
  selector: 'app-seeker-profile',
  standalone: true,
  imports: [ReactiveFormsModule, SkillRowComponent,NavbarComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;

  profile: SeekerProfile | null = null;
  isLoading = true;
  isSaving = false;
  profileExists = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private fb: FormBuilder,
    private profileService: SeekerProfileService,
    private route: ActivatedRoute
  ) {
    this.profileForm = this.fb.group({
      skills: this.fb.array<FormControl<string>>([
        this.createSkillControl()
      ]),
      experienceYears: this.fb.nonNullable.control(0, [
        Validators.required,
        Validators.min(0),
        Validators.max(50)
      ]),
      education: this.fb.nonNullable.control('', [
        Validators.required,
        Validators.maxLength(200)
      ]),
      location: this.fb.nonNullable.control('', [
        Validators.maxLength(100)
      ])
    });
  }

  ngOnInit(): void {
    this.loadProfile();
  }

  get skills(): FormArray<FormControl<string>> {
    return this.profileForm.get('skills') as FormArray<FormControl<string>>;
  }

  get skillControls(): FormControl<string>[] {
    return this.skills.controls;
  }

  addSkill(value = ''): void {
    this.skills.push(this.createSkillControl(value));
  }

  removeSkill(index: number): void {
    if (this.skills.length <= 1) {
      return;
    }

    this.skills.removeAt(index);
  }

  submitProfile(): void {
    this.errorMessage = '';
    this.successMessage = '';

    this.profileForm.markAllAsTouched();

    const cleanedSkills = this.skillControls
      .map(control => control.value.trim())
      .filter(skill => skill.length > 0);

    if (cleanedSkills.length === 0) {
      this.errorMessage = 'Please enter at least one skill.';
      return;
    }

    if (this.profileForm.invalid) {
      this.errorMessage = 'Please correct the form errors before saving.';
      return;
    }

    const request: SeekerProfileRequest = {
      skills: cleanedSkills,
      experienceYears: Number(this.profileForm.value.experienceYears ?? 0),
      education: String(this.profileForm.value.education ?? '').trim(),
      location: String(this.profileForm.value.location ?? '').trim()
    };

    this.isSaving = true;

    const saveRequest = this.profileExists
      ? this.profileService.updateProfile(request)
      : this.profileService.createProfile(request);

    saveRequest.subscribe({
      next: (profile) => {
        this.profile = profile;
        this.profileExists = true;
        this.isSaving = false;
        this.successMessage = 'Profile saved successfully.';
        this.setFormValues(profile);
      },
      error: (error: HttpErrorResponse) => {
        this.isSaving = false;
        this.errorMessage = this.getErrorMessage(
          error,
          'Unable to save profile.'
        );
      }
    });
  }

  private loadProfile(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.profileService.getProfile().subscribe({
      next: (profile) => {
        this.profile = profile;
        this.profileExists = true;
        this.setFormValues(profile);
        this.addSkillFromQueryParameter();
        this.isLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.isLoading = false;

        if (error.status === 404) {
          this.profileExists = false;
          this.addSkillFromQueryParameter();
          return;
        }

        this.errorMessage = this.getErrorMessage(
          error,
          'Unable to load profile.'
        );
      }
    });
  }

  private setFormValues(profile: SeekerProfile): void {
    this.skills.clear();

    if (profile.skills.length > 0) {
      profile.skills.forEach(skill => this.addSkill(skill));
    } else {
      this.addSkill();
    }

    this.profileForm.patchValue({
      experienceYears: profile.experienceYears,
      education: profile.education,
      location: profile.location
    });
  }

  private addSkillFromQueryParameter(): void {
    const suggestedSkill = this.route.snapshot.queryParamMap
      .get('skill')
      ?.trim();

    if (!suggestedSkill) {
      return;
    }

    const alreadyExists = this.skillControls.some(
      control => control.value.trim().toLowerCase() === suggestedSkill.toLowerCase()
    );

    if (alreadyExists) {
      return;
    }

    const firstSkill = this.skillControls[0];
    if (this.skills.length === 1 && firstSkill.value.trim() === '') {
      firstSkill.setValue(suggestedSkill);
    } else {
      this.addSkill(suggestedSkill);
    }
  }

  private createSkillControl(value = ''): FormControl<string> {
    return this.fb.nonNullable.control(value, [Validators.required]);
  }

  private getErrorMessage(
    error: HttpErrorResponse,
    fallbackMessage: string
  ): string {
    if (typeof error.error?.message === 'string') {
      return error.error.message;
    }

    return fallbackMessage;
  }
}
