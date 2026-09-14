import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'matchBand',
  standalone: true
})
export class MatchBandPipe implements PipeTransform {

  transform(score: number): string {

    if (score >= 80) {
      return 'Strong Match';
    }

    if (score >= 60) {
      return 'Good Match';
    }

    if (score >= 40) {
      return 'Moderate Match';
    }

    return 'Low Match';
  }

}
