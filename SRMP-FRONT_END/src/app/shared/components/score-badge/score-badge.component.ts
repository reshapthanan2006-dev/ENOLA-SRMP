import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatchBandPipe } from '../../pipes/match-band.pipe';
import { ScoreColorDirective } from '../../directives/score-color.directive';

@Component({
  selector: 'app-score-badge',
  standalone: true,
  imports: [
    CommonModule,
    MatchBandPipe,
    ScoreColorDirective
  ],
  templateUrl: './score-badge.component.html',
  styleUrl: './score-badge.component.css'
})
export class ScoreBadgeComponent {

  @Input({ required: true }) score!: number;

  @Input() mediumMinimum: number | null = null;

  @Input() highMinimum: number | null = null;
}