export interface DailyTask {
  id: string;
  createdAt: string;
  updatedAt: string;
  date: string;
  animalId: string;
  entries: DailyTaskEntry[];
}

export interface DailyTaskEntry {
  id: string;
  createdAt: string;
  updatedAt: string;
  title: string;
  description?: string;
  isCompleted: boolean;
  completedAt?: string;
  dailyTaskId: string;
  userId?: string;
  userDisplayName?: string;
}

export interface CreateDailyTaskEntry {
  title: string;
  description?: string;
}
