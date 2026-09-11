import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'shortText',
  standalone: true
})
export class ShortTextPipe implements PipeTransform {

  transform(text: string, limit: number = 100): string {

    if (text.length <= limit) {
      return text;
    }

    return text.substring(0, limit) + '...';
  }

}