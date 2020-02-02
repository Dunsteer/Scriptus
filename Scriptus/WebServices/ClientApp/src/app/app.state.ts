import { AuthStateManager } from "@store/auth/state";
import { UserStateManager } from "@store/user/state";
import { CommentStateManager } from "@store/comment/state";
import { PostStateManager } from "@store/post/state";

export const AppState = [
  AuthStateManager,
  UserStateManager,
  CommentStateManager,
  PostStateManager
];
