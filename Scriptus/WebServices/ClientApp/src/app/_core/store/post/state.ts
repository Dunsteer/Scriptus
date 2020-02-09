import { State, Action, StateContext, Selector } from "@ngxs/store";
import { AuthService } from "@services/auth.service";
import { PostActions } from "./actions";
import { map, catchError } from "rxjs/operators";
import { of } from "rxjs";
import { User } from "@models/user.model";
import {
  patch,
  updateItem,
  insertItem,
  removeItem
} from "@ngxs/store/operators";
import { eUserRank } from "../../enumerators/user-rank.enum";
import { CookieService } from "ngx-cookie-service";
import { UserService } from "@services/user.service";
import { PostService } from "@services/post.service";
import { Post } from "@models/post.model";

export interface PostState {
  post: Post;
  posts: Post[];
  uploadedImages: string[];
  uploadedPdf: string;
}

const initialState: PostState = {
  post: null,
  posts: [],
  uploadedImages: [],
  uploadedPdf: null
};

@State<PostState>({ name: "post", defaults: initialState })
export class PostStateManager {
  constructor(private _post: PostService) {}

  @Action(PostActions.Fetch)
  fetch(ctx: StateContext<PostState>, action: PostActions.Fetch) {
    return this._post.fetch(action.id).pipe(
      map(res => {
        return ctx.patchState({ post: res });
      }),
      catchError(err => {
        console.error(err);
        throw err;
      })
    );
  }

  @Action(PostActions.Search)
  search(ctx: StateContext<PostState>, action: PostActions.Search) {
    return this._post.search(action.tags).pipe(
      map(res => {
        return ctx.patchState({ posts: res.list });
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Action(PostActions.Remove)
  remove(ctx: StateContext<PostState>, action: PostActions.Remove) {
    return this._post.remove(action.id).pipe(
      map(res => {
        return ctx.setState(
          patch<PostState>({
            posts: removeItem<Post>(x => x.id == action.id)
          })
        );
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Action(PostActions.Create)
  create(ctx: StateContext<PostState>, action: PostActions.Create) {
    return this._post.create(action.post).pipe(
      map(res => {
        ctx.patchState({ post: res });
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Action(PostActions.VoteUp)
  voteUp(ctx: StateContext<PostState>, action: PostActions.VoteUp) {
    return this._post.voteUp(action.id).pipe(
      map(res => {
        return ctx.patchState({
          post: res
        });
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Action(PostActions.VoteDown)
  voteDown(ctx: StateContext<PostState>, action: PostActions.VoteDown) {
    return this._post.voteDown(action.id).pipe(
      map(res => {
        return ctx.patchState({
          post: res
        });
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Action(PostActions.VoteUpComment)
  voteUpComment(
    ctx: StateContext<PostState>,
    action: PostActions.VoteUpComment
  ) {
    return this._post.voteUpComment(action.id, action.parentId).pipe(
      map(res => {
        return ctx.setState(
          patch<PostState>({
            post: patch<Post>({
              comments: updateItem<Post>(x => x.id == action.id, res)
            })
          })
        );
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Action(PostActions.VoteDownComment)
  voteDownComment(
    ctx: StateContext<PostState>,
    action: PostActions.VoteDownComment
  ) {
    return this._post.voteDownComment(action.id, action.parentId).pipe(
      map(res => {
        return ctx.setState(
          patch<PostState>({
            post: patch<Post>({
              comments: updateItem<Post>(x => x.id == action.id, res)
            })
          })
        );
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Action(PostActions.FilesUpload)
  filesUpload(ctx: StateContext<PostState>, action: PostActions.FilesUpload) {
    return this._post.uploadFiles(action.files).pipe(
      map(res => {
        return ctx.setState(
          patch<PostState>({
            uploadedImages: res.paths
          })
        );
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Action(PostActions.FileUpload)
  fileUpload(ctx: StateContext<PostState>, action: PostActions.FileUpload) {
    return this._post.uploadFile(action.file).pipe(
      map(res => {
        console.log(res);
        return ctx.setState(
          patch<PostState>({
            uploadedPdf: res.paths[0]
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
  static state(state: PostState) {
    return state;
  }

  @Selector()
  static post(state: PostState) {
    return state.post;
  }
}
