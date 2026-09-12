import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-skill-row',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './skill-row.component.html',
  styleUrl: './skill-row.component.css'
})
export class SkillRowComponent {
  @Input({ required: true }) control!: FormControl<string>;
  @Input() index = 0;
  @Input() canRemove = true;

  @Output() remove = new EventEmitter<void>();

  removeSkill(): void {
    this.remove.emit();
  }
}
