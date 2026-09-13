import { DatePipe } from '@angular/common';
import {
  Component,
  EventEmitter,
  Input,
  Output
} from '@angular/core';

import {
  AppNotification
} from '../../models/notification.model';

@Component({
  selector: 'app-notification-item',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './notification-item.component.html',
  styleUrl: './notification-item.component.css'
})
export class NotificationItemComponent {

  @Input({ required: true })
  notification!: AppNotification;

  @Input()
  isMarkingRead = false;

  @Output()
  markRead = new EventEmitter<number>();

  onMarkAsRead(): void {

    if (
      this.notification.isRead ||
      this.isMarkingRead
    ) {
      return;
    }

    this.markRead.emit(
      this.notification.notificationId
    );
  }
}