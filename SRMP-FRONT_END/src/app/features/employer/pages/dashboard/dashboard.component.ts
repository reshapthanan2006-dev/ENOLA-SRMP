import {
  Component,
  OnInit,
  inject
} from '@angular/core';

import {
  Router,
  RouterLink
} from '@angular/router';

import { AuthService } from '../../../../core/auth/auth.service';

import { Company } from '../../models/company.model';
import { CompanyService } from '../../services/company.service';
import { VacancyService } from '../../services/vacancy.service';

@Component({
  selector: 'app-employer-dashboard',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {

  private readonly authService =
    inject(AuthService);

  private readonly router =
    inject(Router);

  private readonly companyService =
    inject(CompanyService);

  private readonly vacancyService =
    inject(VacancyService);

  company: Company | null = null;

  totalVacancies = 0;
  openVacancies = 0;
  closedVacancies = 0;

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

  logout(): void {
    this.authService.logout();

    this.router.navigate([
      '/login'
    ]);
  }
}