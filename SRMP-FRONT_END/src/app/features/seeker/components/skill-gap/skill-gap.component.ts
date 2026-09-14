import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-skill-gap',
  standalone: true,
  templateUrl: './skill-gap.component.html',
  styleUrl: './skill-gap.component.css'
})
export class SkillGapComponent {
  @Input({ required: true }) missingSkills!: string[];
  @Output() skillSelected = new EventEmitter<string>();

  selectSkill(skill: string): void {
    this.skillSelected.emit(skill);
  }
}
