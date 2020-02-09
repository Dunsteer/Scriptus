import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import { AuthActions } from "@store/auth/actions";
import { AuthStateManager } from "@store/auth/state";
import { Observable } from "rxjs";
import { User } from "@models/user.model";
import { Select } from "@ngxs/store";
import { Post } from "@models/post.model";

export class BaseComponent {
  @Select(AuthStateManager.user) user$: Observable<User>;
  constructor() {}

  reputation(post: Post): number {
    return post.voteUp.length - post.voteDown.length;
  }

  hasPermission(user: User) {
    return user.rank == 1;
  }

  clearSearch() {
    (document.querySelector("#searchInput") as HTMLInputElement).value = "";
  }
}
