import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-score-badge',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './score-badge.component.html',
  styleUrl: './score-badge.component.css'
})
export class ScoreBadgeComponent {

  @Input({ required: true }) score!: number;

}