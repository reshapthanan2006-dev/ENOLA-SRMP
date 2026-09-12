import { Component, OnInit } from '@angular/core';

import { Company } from '../../models/company.model';
import { Vacancy } from '../../models/vacancy.model';

@Component({
  selector: 'app-employer-dashboard',
  standalone: true,
  imports: [],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {

  company: Company | null = null;

  totalVacancies = 0;
  openVacancies = 0;
  closedVacancies = 0;

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {

    const savedCompany =
      localStorage.getItem('member3CompanyProfile');

    if (savedCompany) {
      this.company = JSON.parse(savedCompany);
    }


    const savedVacancies =
      localStorage.getItem('member3Vacancies');

    let vacancies: Vacancy[] = [];

    if (savedVacancies) {
      vacancies = JSON.parse(savedVacancies);
    }

    this.totalVacancies = vacancies.length;

    this.openVacancies =
      vacancies.filter(
        vacancy => vacancy.isOpen
      ).length;

    this.closedVacancies =
      vacancies.filter(
        vacancy => !vacancy.isOpen
      ).length;
  }
}