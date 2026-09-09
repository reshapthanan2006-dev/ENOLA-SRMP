import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'experienceYears',
  standalone: true
})
export class ExperienceYearsPipe implements PipeTransform {

  transform(years: number): string {

    if (years === 1) {
      return '1 year';
    }

    return `${years} years`;
  }

}