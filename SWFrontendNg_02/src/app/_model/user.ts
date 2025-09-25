export interface User {
  Id: number;
  Username: string;
  Email: string;
  Role: string;
  Token: string;
  RefreshToken: string;
  Requires2FA: boolean;
}
