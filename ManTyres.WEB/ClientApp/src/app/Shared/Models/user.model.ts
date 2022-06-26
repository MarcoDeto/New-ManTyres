import { MongoDocument } from "./base.model";
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

export interface User extends MongoDocument {
  email: string | null,
  phoneNumber: string | null,
  concurrencyStamp: string | null,
  securityStamp: string | null,
  passwordHash: string | null,
  userName: string | null,
  firstName: string | null,
  lastName: string | null,
  photoUrl: string | null,
  companyName: string | null,
  street: string | null,
  zipcode: string | null,
  country: string | null,
  city: string | null,
  website: string | null,
  cultureInfo: string | null,
  provider: string | null,
  socialUserId: string | null,
  userRole: UserRole,
  twoFactorEnabled: boolean,
  phoneNumberConfirmed: boolean,
  emailConfirmed: boolean,
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
