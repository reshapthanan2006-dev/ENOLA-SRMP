import { Component, OnInit } from '@angular/core';

import { Company } from '../../models/company.model';
import { CompanyService } from '../../services/company.service';
import { VacancyService } from '../../services/vacancy.service';

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

  constructor(
    private companyService: CompanyService,
    private vacancyService: VacancyService
  ) { }

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {

    this.company =
      this.companyService.getCompany();

    const vacancies =
      this.vacancyService.getVacancies();

    this.totalVacancies =
      vacancies.length;

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