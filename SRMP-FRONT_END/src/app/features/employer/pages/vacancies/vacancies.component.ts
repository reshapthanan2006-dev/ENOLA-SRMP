import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Vacancy } from '../../models/vacancy.model';

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
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadVacancies();
  }

  loadVacancies(): void {
    const savedVacancies =
      localStorage.getItem('member3Vacancies');

    if (savedVacancies) {
      this.vacancies = JSON.parse(savedVacancies);
    } else {
      this.vacancies = [];
    }
  }

  openCreateVacancy(): void {
    this.router.navigate([
      '/employer/vacancies/new'
    ]);
  }

  closeVacancy(vacancyId: number): void {

    const vacancy =
      this.vacancies.find(
        item => item.jobVacancyId === vacancyId
      );

    if (!vacancy) {
      return;
    }

    vacancy.isOpen = false;

    localStorage.setItem(
      'member3Vacancies',
      JSON.stringify(this.vacancies)
    );
  }
  openEditVacancy(vacancyId: number): void {
  this.router.navigate([
    '/employer/vacancies/edit',
    vacancyId
  ]);
}
}