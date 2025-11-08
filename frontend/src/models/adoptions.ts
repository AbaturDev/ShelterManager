export type AdoptionStatus = 0 | 1 | 2;
export const PossibleAdoptionStatus = {
  Active: 0 as AdoptionStatus,
  Approved: 1 as AdoptionStatus,
  Rejected: 2 as AdoptionStatus,
};

export interface Adoption {
  id: string;
  status: AdoptionStatus;
  animalName: string;
  animalId: string;
  startAdoptionProcess: string;
  adoptionDate?: string;
}

export interface AdoptionDetails {
  id: string;
  status: AdoptionStatus;
  animalName: string;
  animalId: string;
  startAdoptionProcess: string;
  adoptionDate?: string;
  createdAt: string;
  updatedAt: string;
  person: AdoptionPerson;
  note?: string;
}

export interface AdoptionPerson {
  name: string;
  surname: string;
  phoneNumber: string;
  pesel: string;
  documentId: string;
  email: string;
  city: string;
  street: string;
  postalCode: string;
}

export interface CreateAdoption {
  note?: string;
  animalId: string;
  person: AdoptionPerson;
}

export interface UpdateAdoption {
  status: AdoptionStatus;
  note?: string;
  event?: AdoptionEvent;
}

export interface AdoptionEvent {
  plannedAdoptionDate: string;
  title: string;
  location: string;
  description?: string;
}

export interface AdoptionQuery {
  page: number;
  pageSize: number;
  animalIds?: string[];
  animalName?: string;
  status?: AdoptionStatus[];
}
