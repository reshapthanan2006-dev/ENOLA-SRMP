import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationStateService {

  private readonly unreadCountSubject =
    new BehaviorSubject<number>(0);

  readonly unreadCount$ =
    this.unreadCountSubject.asObservable();

  setUnreadCount(
    count: number
  ): void {

    this.unreadCountSubject.next(
      Math.max(0, count)
    );
  }

  decrementUnreadCount(): void {

    const currentCount =
      this.unreadCountSubject.value;

    this.unreadCountSubject.next(
      Math.max(0, currentCount - 1)
    );
  }

  resetUnreadCount(): void {

    this.unreadCountSubject.next(0);
  }
}