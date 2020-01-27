import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import { AuthActions } from "@store/auth/actions";
import { AuthStateManager } from '@store/auth/state';
import { Observable } from 'rxjs';
import { User } from '@models/user.model';
import { Select } from '@ngxs/store';

export class BaseComponent {
  @Select(AuthStateManager.user) user$: Observable<User>;
  constructor() { }
}
