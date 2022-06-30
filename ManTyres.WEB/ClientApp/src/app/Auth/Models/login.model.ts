export interface LoginModel{
  email: string;
  password: string;
}

export class LoginResponse {
  constructor(
    public token: string,
    public expiration: string
  ) { }
}
