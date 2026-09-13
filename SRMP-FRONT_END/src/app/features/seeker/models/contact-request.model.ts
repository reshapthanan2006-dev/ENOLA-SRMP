export type ContactRequestStatus =
  | 'Pending'
  | 'Accepted'
  | 'Declined';

export interface ContactRequest {
  contactRequestId: number;

  employerId: number;

  jobSeekerId: number;

  applicationId: number;

  jobVacancyId: number;

  companyName: string;

  jobTitle: string;

  jobLocation: string;

  applicationStatus: string;

  status: ContactRequestStatus;

  createdAt: string;
}

export interface CreateContactRequest {
  jobSeekerId: number;
  applicationId: number;
}