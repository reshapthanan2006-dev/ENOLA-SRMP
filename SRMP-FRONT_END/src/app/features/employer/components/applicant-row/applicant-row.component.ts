import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ScoreBadgeComponent } from '../../../../shared/components/score-badge/score-badge.component';
import { SkillListPipe } from '../../../../shared/pipes/skill-list.pipe';
import { MatchResult } from '../../../matching/models/match-result.model';

@Component({
  selector: 'app-applicant-row',
  standalone: true,
  imports: [ScoreBadgeComponent, SkillListPipe],
  templateUrl: './applicant-row.component.html',
  styleUrl: './applicant-row.component.css'
})
export class ApplicantRowComponent {
  @Input({ required: true }) applicant!: MatchResult;
  @Input({ required: true }) applicationStatuses!: string[];
  @Input({ required: true }) updatingApplicationId!: number | null;
  @Input({ required: true }) sendingContactApplicationId!: number | null;
  @Input({ required: true }) contactRequestSent!: boolean;

  @Output() viewProfile = new EventEmitter<number>();
  @Output() contactCandidate = new EventEmitter<MatchResult>();
  @Output() statusChanged = new EventEmitter<{
    applicationId: number;
    status: string;
  }>();

  onStatusChanged(status: string): void {
    this.statusChanged.emit({
      applicationId: this.applicant.applicationId,
      status: status
    });
  }
}
