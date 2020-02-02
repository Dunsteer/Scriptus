import { eUserRank } from "../enumerators/user-rank.enum";

export interface User {
  id?: string;
  username?: string;
  rank?: eUserRank;
  fullName?: string;
  email?: string;
  additionalEmails?: string[];
  password?: string;
  confirmPassword?: string;
  token?: string;
}
