import { Component, OnInit } from "@angular/core";
import { Store } from "@ngxs/store";
import { CookieService } from "ngx-cookie-service";
import { AuthActions } from "@store/auth/actions";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html"
})
export class AppComponent implements OnInit {
  constructor(private _store: Store, private _cookie: CookieService) {}

  ngOnInit(): void {
    if (this._cookie.check("token")) {
      this._store.dispatch(new AuthActions.Check());
    }
  }
}
