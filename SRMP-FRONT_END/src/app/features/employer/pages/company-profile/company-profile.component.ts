import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  Company,
  CompanyRequest
} from '../../models/company.model';

import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-company-profile',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    DatePipe
  ],
  templateUrl: './company-profile.component.html',
  styleUrl: './company-profile.component.css'
})
export class CompanyProfileComponent implements OnInit {

  company: Company | null = null;

  isLoading = true;
  isSaving = false;
  isFormVisible = false;

  errorMessage = '';
  saveErrorMessage = '';
  successMessage = '';

  companyForm = new FormGroup({
    companyName: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.maxLength(150)
      ]
    }),

    description: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.maxLength(1000)
      ]
    })
  });

  ngOnInit(): void {
    this.loadCompany();
  }

  loadCompany(): void {
    this.isLoading = true;
    this.errorMessage = '';

    const savedCompany =
      localStorage.getItem('member3CompanyProfile');

    if (savedCompany) {
      this.company = JSON.parse(savedCompany);
    } else {
      this.company = null;
    }

    this.isLoading = false;
  }

  openCreateForm(): void {
    this.companyForm.reset({
      companyName: '',
      description: ''
    });

    this.saveErrorMessage = '';
    this.successMessage = '';
    this.isFormVisible = true;
  }

  openEditForm(): void {
    if (!this.company) {
      return;
    }

    this.companyForm.setValue({
      companyName: this.company.companyName,
      description: this.company.description
    });

    this.saveErrorMessage = '';
    this.successMessage = '';
    this.isFormVisible = true;
  }

  cancelForm(): void {
    this.isFormVisible = false;
    this.saveErrorMessage = '';
  }

  saveCompany(): void {

    if (this.companyForm.invalid) {
      this.companyForm.markAllAsTouched();
      return;
    }

    const companyRequest: CompanyRequest =
      this.companyForm.getRawValue();

    this.isSaving = true;
    this.saveErrorMessage = '';
    this.successMessage = '';

    if (this.company) {

      const updatedCompany: Company = {
        ...this.company,
        companyName: companyRequest.companyName,
        description: companyRequest.description,
        updatedAt: new Date().toISOString()
      };

      this.company = updatedCompany;

      localStorage.setItem(
        'member3CompanyProfile',
        JSON.stringify(updatedCompany)
      );

      this.successMessage =
        'Company profile updated successfully.';

    } else {

      const createdCompany: Company = {
        employerCompanyId: 1,
        companyName: companyRequest.companyName,
        description: companyRequest.description,
        employerId: 1,
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString()
      };

      this.company = createdCompany;

      localStorage.setItem(
        'member3CompanyProfile',
        JSON.stringify(createdCompany)
      );

      this.successMessage =
        'Company profile created successfully.';
    }

    this.isSaving = false;
    this.isFormVisible = false;
  }
}