import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import { AuthActions } from "@store/auth/actions";
import { BaseComponent } from "src/app/_core/components/base.component";
import { Store } from "@ngxs/store";
import { Router } from "@angular/router";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.scss"]
})
export class RegisterComponent extends BaseComponent implements OnInit {
  registerForm: FormGroup;

  constructor(public _store: Store, public _router: Router) {
    super();
  }

  get fullName() {
    return this.registerForm.get("fullName");
  }

  get email() {
    return this.registerForm.get("email");
  }

  get username() {
    return this.registerForm.get("username");
  }

  get password() {
    return this.registerForm.get("password");
  }

  get confirmPassword() {
    return this.registerForm.get("confirmPassword");
  }

  ngOnInit() {
    this.registerForm = new FormGroup({
      fullName: new FormControl(""),
      email: new FormControl(""),
      username: new FormControl(""),
      password: new FormControl(""),
      confirmPassword: new FormControl("")
    });
  }

  submit(e) {
    //e.preventDefault();
    this._store
      .dispatch(new AuthActions.Register(this.registerForm.value))
      .subscribe(
        () => {
          this._router.navigateByUrl("/");
        },
        err => {
          console.error(err);
        }
      );
  }
}
