import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'matchBand',
  standalone: true
})
export class MatchBandPipe implements PipeTransform {

  transform(
    score: number,
    mediumMinimum: number,
    highMinimum: number
  ): string {

    if (score >= highMinimum) {
      return 'High Match';
    }

    if (score >= mediumMinimum) {
      return 'Medium Match';
    }

    return 'Low Match';
  }
}