import { User } from "./user.model";

export interface Comment {
  id?: string;
  parentId?: string;
  userId?: string;
  user?: User;
  date?: Date;
  data?: string;
  replies?: Comment[];
}
