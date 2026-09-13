import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-skill-gap-panel',
  standalone: true,
  templateUrl: './skill-gap-panel.component.html',
  styleUrl: './skill-gap-panel.component.css'
})
export class SkillGapPanelComponent {

  @Input() missingSkills: string[] = [];

  @Output() skillSelected = new EventEmitter<string>();

  selectSkill(skill: string): void {
    this.skillSelected.emit(skill);
  }

}