export interface Breed {
  id: string;
  name: string;
  createdAt: string;
  updatedAt: string;
  speciesId: string;
}

export interface CreateBreed {
  name: string;
}
