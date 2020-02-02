import { User } from "@models/user.model";

export namespace UserActions {
  export class Fetch {
    static readonly type = "[USER] Fetch";
    constructor(public id: string) {}
  }

  export class FetchTop {
    static readonly type = "[USER] Fetch top";
    constructor(public from: number, public to: number) {}
  }
}
