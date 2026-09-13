import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Vacancy } from '../../models/vacancy.model';
import { VacancyService } from '../../services/vacancy.service';

@Component({
  selector: 'app-vacancies',
  standalone: true,
  imports: [],
  templateUrl: './vacancies.component.html',
  styleUrl: './vacancies.component.css'
})
export class VacanciesComponent implements OnInit {

  vacancies: Vacancy[] = [];

  constructor(
    private router: Router,
    private vacancyService: VacancyService
  ) { }

  ngOnInit(): void {
    this.loadVacancies();
  }

  loadVacancies(): void {

    this.vacancies =
      this.vacancyService.getVacancies();
  }

  openCreateVacancy(): void {

    this.router.navigate([
      '/employer/vacancies/new'
    ]);
  }

  closeVacancy(
    vacancyId: number
  ): void {

    const confirmed =
      window.confirm(
        'Are you sure you want to close this vacancy?'
      );

    if (!confirmed) {
      return;
    }

    const closed =
      this.vacancyService.closeVacancy(
        vacancyId
      );

    if (closed) {
      this.loadVacancies();
    }
  }

  openEditVacancy(
    vacancyId: number
  ): void {

    this.router.navigate([
      '/employer/vacancies/edit',
      vacancyId
    ]);
  }

}