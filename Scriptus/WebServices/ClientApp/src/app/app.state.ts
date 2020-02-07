import { AuthStateManager } from "@store/auth/state";
import { UserStateManager } from "@store/user/state";
import { PostStateManager } from "@store/post/state";

export const AppState = [AuthStateManager, UserStateManager, PostStateManager];
