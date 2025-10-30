export interface UserDetails {
  id: string;
  email: string;
  mustChangePassword: boolean;
  name: string;
  surname: string;
  role: string;
}

export interface User {
  id: string;
  email: string;
  name: string;
  surname: string;
}
