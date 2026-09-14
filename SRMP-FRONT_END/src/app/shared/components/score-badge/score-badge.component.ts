import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatchBandPipe } from '../../pipes/match-band.pipe';

@Component({
  selector: 'app-score-badge',
  standalone: true,
  imports: [CommonModule, MatchBandPipe],
  templateUrl: './score-badge.component.html',
  styleUrl: './score-badge.component.css'
})
export class ScoreBadgeComponent {

  @Input({ required: true }) score!: number;

}
