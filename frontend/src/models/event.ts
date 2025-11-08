export interface Event {
  id: string;
  createdAt: string;
  updatedAt: string;
  title: string;
  description?: string;
  date: string;
  isDone: true;
  completedAt?: string;
  cost?: Money;
  location: string;
  animalName: string;
  animalId: string;
  userId?: string;
  completedByUserId?: string;
}

export interface Money {
  amount: number;
  currencyCode: string;
}

export interface EventQuery {
  page: number;
  pageSize: number;
  title?: string;
  animalIds?: string[];
  isDone?: boolean;
}

export interface CreateEvent {
  title: string;
  description?: string;
  date: string;
  cost?: Money;
  location: string;
  animalId: string;
  userId: string;
}

export interface UpdateEvent {
  title: string;
  description?: string;
  date: string;
  cost?: Money;
  location: string;
  userId: string;
}
