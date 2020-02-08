import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import { AuthActions } from "@store/auth/actions";
import { BaseComponent } from "src/app/_core/components/base.component";
import { Store, Select } from "@ngxs/store";
import { Router, ActivatedRoute } from "@angular/router";
import { UserStateManager } from "@store/user/state";
import { Observable } from "rxjs";
import { User } from "@models/user.model";
import { UserActions } from "@store/user/actions";

@Component({
  selector: "app-user",
  templateUrl: "./user.component.html",
  styleUrls: ["./user.component.scss"]
})
export class UserComponent extends BaseComponent implements OnInit {
  @Select(UserStateManager.user) user$: Observable<User>;
  id: string;
  found = true;

  constructor(
    public _store: Store,
    public _router: Router,
    public _route: ActivatedRoute
  ) {
    super();
  }

  ngOnInit() {
    this._route.paramMap.subscribe(params => {
      this.id = params.get("id");
      this._store.dispatch(new UserActions.Fetch(this.id)).subscribe(
        () => {
          this.found = true;
        },
        err => {
          this.found = false;
        }
      );
    });

    this.clearSearch();
  }
}
