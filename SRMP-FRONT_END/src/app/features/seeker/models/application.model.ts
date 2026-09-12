export interface Application {
  applicationId: number;
  jobSeekerId: number;
  jobVacancyId: number;
  status: string;
  appliedAt: string;
}

export interface CreateApplication {
  jobVacancyId: number;
}

export interface UpdateApplicationStatus {
  status: string;
}