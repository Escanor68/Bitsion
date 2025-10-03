export interface LoginDto {
  username: string;
  password: string;
}

export interface LoginResponseDto {
  token: string;
  username: string;
  role: string;
  message: string;
}
