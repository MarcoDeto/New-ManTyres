import { UserRole } from "./enums";

export interface Users {
  id: string | null,
  imgProfile: string | null,
  userName: string | null,
  email: string | null,
  phoneNumber: string | null,
  firstName: string | null,
  lastName: string | null,
  isDeleted: boolean,
}

export class User {
  constructor(
    public id: string | null,
    public email: string | null,
    public phoneNumber: string | null,
    public concurrencyStamp: string | null,
    public securityStamp: string | null,
    public passwordHash: string | null,
    public userName: string | null,
    public firstName: string | null,
    public lastName: string | null,
    public photoUrl: string | null,
    public companyName: string | null,
    public street: string | null,
    public zipcode: string | null,
    public country: string | null,
    public city: string | null,
    public website: string | null,
    public cultureInfo: string | null,
    public provider: string | null,
    public userRole: UserRole,
    public twoFactorEnabled: boolean,
    public phoneNumberConfirmed: boolean,
    public emailConfirmed: boolean,
    public isDeleted: boolean,
  ) { }
}

export class Utenza {
  constructor(
    public id: string | null,
    public imgProfile: string | null,
    public userName: string | null,
    public firstName: string | null,
    public lastName: string | null,
    public email: string | null,
    public emailConfirmed: boolean,
    public phoneNumber: string | null,
    public password: string | null,
    public confirmPassword: string | null,
    public role: string | null,
    public isDeleted: boolean
  ) { }
}

export class UserPassword {
  constructor(
    public id: string | null,
    public userName: string | null,
    public password: string | null,
    public newPassword: string | null,
  ) { }
}

export class UserName {
  constructor(
    public id: string | null,
    public userName: string | null,
    public firstName: string | null,
    public lastName: string | null,
    public email: string | null,
    public phoneNumber: string | null
  ) { }
}

export class UserFilter {
  constructor(
    public firstName: string | null,
    public lastName: string | null,
    public userName: string | null,
    public email: string | null
  ) { }
}
