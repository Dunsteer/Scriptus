import { Component } from "@angular/core";
import { BaseComponent } from "src/app/_core/components/base.component";
import { Router } from "@angular/router";
import { Store } from "@ngxs/store";
import { AuthActions } from "@store/auth/actions";

@Component({
  selector: "app-nav-menu",
  templateUrl: "./nav-menu.component.html",
  styleUrls: ["./nav-menu.component.scss"]
})
export class NavMenuComponent extends BaseComponent {
  constructor(private _router: Router, private _store: Store) {
    super();
  }
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
  search(text: string) {
    text = text.trim();
    this._router.navigateByUrl(`/${text}`);
  }
  logout() {
    this._store.dispatch(new AuthActions.Logout());
  }
}
