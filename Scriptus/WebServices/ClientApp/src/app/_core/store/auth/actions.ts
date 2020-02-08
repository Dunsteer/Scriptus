import { User } from '@models/user.model';

export namespace AuthActions {
  export class Register {
    static readonly type = "[AUTH] Register";
    constructor(public user: User) { }
  }

  export class Login {
    static readonly type = "[AUTH] Login";
    constructor(public user: User) { }
  }

  export class Check {
    static readonly type = "[AUTH] Check";
    constructor() { }
  }

  export class Logout {
    static readonly type = "[AUTH] Logout";
  }
}
