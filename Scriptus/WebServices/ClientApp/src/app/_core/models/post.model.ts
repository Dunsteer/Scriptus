import { User } from "./user.model";
import { ePostType } from "../enumerators/post-type.enum";

export interface Post {
  id?: string;
  type?: ePostType;
  userId?: string;
  user?: User;
  date?: Date;
  data?: string;
}
