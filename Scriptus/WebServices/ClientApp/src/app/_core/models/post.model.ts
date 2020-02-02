import { User } from "./user.model";

export interface Post {
  id?: string;
  type?: "exam" | "script";
  userId?: string;
  user?: User;
  date?: Date;
  data?: string;
}
