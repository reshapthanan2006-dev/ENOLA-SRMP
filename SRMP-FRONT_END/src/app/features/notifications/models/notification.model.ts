export interface AppNotification {
  notificationId: number;
  jobSeekerId: number;
  applicationId: number;
  message: string;
  isRead: boolean;
  createdAt: string;
}

export interface MarkNotificationReadResponse {
  message: string;
}