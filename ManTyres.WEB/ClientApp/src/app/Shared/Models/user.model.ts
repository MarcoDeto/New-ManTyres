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
  city: string | null,
  companyName: string | null, 
  concurrencyStamp: string | null, 
  country: string | null, 
  cultureInfo: string | null, 
  email: string | null, 
  emailConfirmed: boolean, 
  firstName: string | null, 
  imgProfile: string | null, 
  lastName: string | null, 
  passwordHash: string | null, 
  phoneNumber: string | null, 
  phoneNumberConfirmed: boolean, 
  photoUrl: string | null, 
  provider: string | null, 
  role: UserRole, 
  securityStamp: string | null, 
  socialUserId: string | null, 
  street: string | null, 
  twoFactorEnabled: boolean, 
  userName: string | null, 
  website: string | null, 
  zipcode: string | null,
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
