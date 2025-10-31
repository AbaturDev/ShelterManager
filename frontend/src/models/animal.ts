export type SexType = 0 | 1;
export const PossibleSex = {
  Male: 0 as SexType,
  Female: 1 as SexType,
};

export type AnimalStatusType = 0 | 1 | 2;
export const PossibleAnimalStatus = {
  InShelter: 0 as AnimalStatusType,
  Adopted: 1 as AnimalStatusType,
  Died: 2 as AnimalStatusType,
};

export interface Animal {
  id: string;
  createdAt: string;
  updatedAt: string;
  sex: SexType;
  name: string;
  admissionDate: string;
  status: AnimalStatusType;
  age?: number;
  imagePath?: string;
  description?: string;
  species: AnimalSpecies;
}

export interface AnimalSpecies {
  id: string;
  name: string;
  breed: AnimalBreed;
}

export interface AnimalBreed {
  id: string;
  name: string;
}

export interface AnimalQuery {
  page: number;
  pageSize: number;
  sex?: SexType;
  status?: AnimalStatusType;
  name?: string;
}

export interface CreateAnimal {
  name: string;
  admissionDate: string;
  sex: SexType;
  age?: number;
  breedId: string;
  description?: string;
}

export interface EditAnimal {
  name: string;
  admissionDate: string;
  age?: number;
  description?: string;
  status: AnimalStatusType;
}
