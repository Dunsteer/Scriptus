import { User } from "./user.model";
import { ePostType } from "../enumerators/post-type.enum";

export interface Post {
  id?: string;
  type?: ePostType;
  name?: string;
  userId?: string;
  user?: User;
  date?: Date;
  tags?: string[];
  text?: string;
  images?: string[];
  numberOfQuestions?: number;
  pdf?: string;
  comments?: Post[];
  answerFor?: number;
  voteUp?: User[];
  voteDown?: User[];
}
