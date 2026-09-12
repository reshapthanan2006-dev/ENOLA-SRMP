export interface Company {
  employerCompanyId: number;
  companyName: string;
  description: string;
  employerId: number;
  createdAt: string;
  updatedAt: string;
}

export interface CompanyRequest {
  companyName: string;
  description: string;
}