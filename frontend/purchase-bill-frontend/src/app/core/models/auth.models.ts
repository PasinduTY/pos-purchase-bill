export interface LoginRequest {
  email: string;
  password: string;
}

export interface Location {
  location_Code: string;
  location_Name: string;
}

export interface LoginResult {
  success: boolean;
  message: string | null;
  locations: Location[] | null;
  token: string | null;
}
