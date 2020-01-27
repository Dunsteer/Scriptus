import { eUserRank } from "../enumerators/user-rank.enum";

export interface User {
  username?: string;
  rank?: eUserRank;
  firstName?: string;
  lastName?: string;
  email?: string;
  additionalEmails?: string[];
  password?: string;
  confirmPassword?: string;
  token?: string;
}
