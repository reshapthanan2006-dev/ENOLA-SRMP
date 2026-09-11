import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'skillList',
  standalone: true
})
export class SkillListPipe implements PipeTransform {

  transform(skills: string[]): string {
    return skills.join(', ');
  }

}