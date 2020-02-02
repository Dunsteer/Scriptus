import { eUserRank } from "../enumerators/user-rank.enum";

export interface User {
  id?: string;
  username?: string;
  rank?: eUserRank;
  fullName?: string;
  email?: string;
  reputation?: number;
  additionalEmails?: string[];
  password?: string;
  confirmPassword?: string;
  token?: string;
}
