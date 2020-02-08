import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import { AuthActions } from "@store/auth/actions";
import { BaseComponent } from "src/app/_core/components/base.component";
import { Store } from "@ngxs/store";
import { Router } from "@angular/router";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"]
})
export class LoginComponent extends BaseComponent implements OnInit {
  loginForm: FormGroup;

  constructor(public _store: Store, public _router: Router) {
    super();
  }

  get username() {
    return this.loginForm.get("username");
  }

  get password() {
    return this.loginForm.get("password");
  }

  ngOnInit() {
    this.loginForm = new FormGroup({
      username: new FormControl(""),
      password: new FormControl("")
    });

    this.clearSearch();
  }

  submit(e) {
    //e.preventDefault();
    this._store.dispatch(new AuthActions.Login(this.loginForm.value)).subscribe(
      () => {
        this._router.navigateByUrl("/");
      },
      err => {
        console.error(err);
      }
    );
  }
}
