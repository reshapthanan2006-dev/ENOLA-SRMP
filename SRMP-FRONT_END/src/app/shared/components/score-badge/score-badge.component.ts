import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ScoreColorDirective } from '../../directives/score-color.directive';
import { MatchBandPipe } from '../../pipes/match-band.pipe';

@Component({
  selector: 'app-score-badge',
  standalone: true,
  imports: [CommonModule, MatchBandPipe, ScoreColorDirective],
  templateUrl: './score-badge.component.html',
  styleUrl: './score-badge.component.css'
})
export class ScoreBadgeComponent {

  @Input({ required: true }) score!: number;

}
