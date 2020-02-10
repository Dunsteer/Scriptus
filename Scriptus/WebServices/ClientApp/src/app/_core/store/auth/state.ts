import { State, Action, StateContext, Selector } from "@ngxs/store";
import { AuthService } from "@services/auth.service";
import { AuthActions } from "./actions";
import { map, catchError } from "rxjs/operators";
import { of } from "rxjs";
import { User } from "@models/user.model";
import { patch, updateItem, insertItem } from "@ngxs/store/operators";
import { eUserRank } from "../../enumerators/user-rank.enum";
import { CookieService } from "ngx-cookie-service";

export interface AuthState {
  user: User;
}

const initialState: AuthState = {
  user: null
};

@State<AuthState>({ name: "auth", defaults: initialState })
export class AuthStateManager {
  constructor(private _auth: AuthService, private _cookie: CookieService) {}

  @Action(AuthActions.Register)
  register(ctx: StateContext<AuthState>, action: AuthActions.Register) {
    return this._auth.register(action.user).pipe(
      map(res => {
        if (res) {
          ctx.dispatch(new AuthActions.Check());
        }
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Action(AuthActions.Login)
  login(ctx: StateContext<AuthState>, action: AuthActions.Login) {
    const state = ctx.getState();
    return this._auth.login(action.user).pipe(
      map(res => {
        if (res) {
          this._cookie.set("token", res.token);
          return ctx.setState({
            ...state,
            user: res.user
          });
        }
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Action(AuthActions.Check)
  check(ctx: StateContext<AuthState>, action: AuthActions.Check) {
    const state = ctx.getState();
    return this._auth.check().pipe(
      map(res => {
        if (res) {
          return ctx.setState({
            ...state,
            user: res
          });
        }
      }),
      catchError(err => {
        console.error(err);
        throw of(err);
      })
    );
  }

  @Action(AuthActions.Logout)
  logout(ctx: StateContext<AuthState>, action: AuthActions.Logout) {
    this._cookie.deleteAll();
    this._cookie.set('toklen', "1", Date.now())
    return ctx.setState({ user: null });
  }

  @Selector()
  static state(state: AuthState) {
    return state;
  }

  @Selector()
  static user(state: AuthState) {
    return state.user;
  }
}
