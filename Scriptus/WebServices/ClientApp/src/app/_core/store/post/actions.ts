import { User } from "@models/user.model";

export namespace PostActions {
  export class Fetch {
    static readonly type = "[POST] Fetch";
    constructor(public id: string) {}
  }

  export class AddComment {
    static readonly type = "[POST] Add comment";
    constructor(public id: string, public data: string) {}
  }
}
