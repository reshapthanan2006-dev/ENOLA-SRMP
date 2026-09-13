import { Injectable } from '@angular/core';

import {
  Company,
  CompanyRequest
} from '../models/company.model';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {

  private readonly storageKey =
    'member3CompanyProfile';

  getCompany(): Company | null {

    const savedCompany =
      localStorage.getItem(this.storageKey);

    if (!savedCompany) {
      return null;
    }

    return JSON.parse(savedCompany);
  }

  createCompany(
    request: CompanyRequest
  ): Company {

    const now =
      new Date().toISOString();

    const company: Company = {
      employerCompanyId: 1,
      companyName: request.companyName,
      description: request.description,
      employerId: 1,
      createdAt: now,
      updatedAt: now
    };

    this.saveCompany(company);

    return company;
  }

  updateCompany(
    request: CompanyRequest
  ): Company | null {

    const existingCompany =
      this.getCompany();

    if (!existingCompany) {
      return null;
    }

    const updatedCompany: Company = {
      ...existingCompany,
      companyName: request.companyName,
      description: request.description,
      updatedAt: new Date().toISOString()
    };

    this.saveCompany(updatedCompany);

    return updatedCompany;
  }

  private saveCompany(
    company: Company
  ): void {

    localStorage.setItem(
      this.storageKey,
      JSON.stringify(company)
    );
  }
}