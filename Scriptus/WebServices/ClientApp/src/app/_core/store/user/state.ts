import { State, Action, StateContext, Selector } from "@ngxs/store";
import { AuthService } from "@services/auth.service";
import { UserActions } from "./actions";
import { map, catchError } from "rxjs/operators";
import { of } from "rxjs";
import { User } from "@models/user.model";
import { patch, updateItem, insertItem } from "@ngxs/store/operators";
import { eUserRank } from "../../enumerators/user-rank.enum";
import { CookieService } from "ngx-cookie-service";
import { UserService } from "@services/user.service";

export interface UserState {
  user: User;
  topUsers: User[];
}

const initialState: UserState = {
  user: null,
  topUsers: []
};

@State<UserState>({ name: "user", defaults: initialState })
export class UserStateManager {
  constructor(private _user: UserService) {}

  @Action(UserActions.Fetch)
  fetch(ctx: StateContext<UserState>, action: UserActions.Fetch) {
    return this._user.fetch(action.id).pipe(
      map(res => {
        ctx.patchState({ user: res });
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Action(UserActions.FetchTop)
  fetchTop(ctx: StateContext<UserState>, action: UserActions.FetchTop) {
    return this._user.fetchTop(action.from, action.to).pipe(
      map(res => {
        ctx.patchState({ topUsers: res });
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Selector()
  static state(state: UserState) {
    return state;
  }

  @Selector()
  static user(state: UserState) {
    return state.user;
  }
}
