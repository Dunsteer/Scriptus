import { State, Action, StateContext, Selector } from "@ngxs/store";
import { AuthService } from "@services/auth.service";
import { CommentActions } from "./actions";
import { map, catchError } from "rxjs/operators";
import { of } from "rxjs";
import { User } from "@models/user.model";
import { patch, updateItem, insertItem, append } from "@ngxs/store/operators";
import { eUserRank } from "../../enumerators/user-rank.enum";
import { CookieService } from "ngx-cookie-service";
import { UserService } from "@services/user.service";
import { CommentService } from "@services/comment.service";
import { Comment } from "@models/comment.model";
import { PostActions } from "@store/post/actions";
import { PostService } from "@services/post.service";

export interface CommentState {
  comments: any[];
  page: number;
}

const initialState: CommentState = {
  comments: [],
  page: -1
};

@State<CommentState>({ name: "comment", defaults: initialState })
export class CommentStateManager {
  constructor(private _comment: CommentService, private _post: PostService) {}

  @Action(CommentActions.Fetch)
  fetch(ctx: StateContext<CommentState>, action: CommentActions.Fetch) {
    const state = ctx.getState();
    return this._comment.fetch(action.id, state.page + 1).pipe(
      map(res => {
        if (state.page == -1) {
          ctx.patchState({ comments: res });
        } else {
          if (res.length > 0) {
            ctx.setState(
              patch<CommentState>({
                comments: append<Comment>(res),
                page: state.page + 1
              })
            );
          }
        }
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Action(CommentActions.FetchReplies)
  fetchReplies(
    ctx: StateContext<CommentState>,
    action: CommentActions.FetchReplies
  ) {
    return this._comment.fetchReplies(action.id).pipe(
      map(res => {
        ctx.setState(
          patch<CommentState>({
            comments: updateItem<Comment>(
              x => x.id == action.id,
              patch<Comment>({ replies: res })
            )
          })
        );
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Action(PostActions.AddComment)
  addComment(ctx: StateContext<CommentState>, action: PostActions.AddComment) {
    return this._post.addComment(action.id, action.data).pipe(
      map(res => {
        return ctx.setState(
          patch<CommentState>({
            comments: insertItem<Comment>(res, 0)
          })
        );
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Action(CommentActions.AddReply)
  addReply(ctx: StateContext<CommentState>, action: CommentActions.AddReply) {
    return this._comment.addReply(action.id, action.data).pipe(
      map(res => {
        return ctx.setState(
          patch<CommentState>({
            comments: updateItem<Comment>(
              x => x.id == action.id,
              patch<Comment>({ replies: insertItem(res, 0) })
            )
          })
        );
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Selector()
  static state(state: CommentState) {
    return state;
  }

  @Selector()
  static comments(state: CommentState) {
    return state.comments;
  }
}
