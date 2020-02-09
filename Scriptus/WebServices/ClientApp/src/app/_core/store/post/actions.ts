import { User } from "@models/user.model";
import { Post } from "@models/post.model";

export namespace PostActions {
  export class Fetch {
    static readonly type = "[POST] Fetch";
    constructor(public id: string) {}
  }

  export class Search {
    static readonly type = "[POST] Search";
    constructor(public tags: string[]) {}
  }

  export class AddComment {
    static readonly type = "[POST] Add comment";
    constructor(public id: string, public data: string) {}
  }

  export class Create {
    static readonly type = "[POST] Create";
    constructor(public post: Post) {}
  }

  export class Remove {
    static readonly type = "[POST] Remove";
    constructor(public id: string) {}
  }

  export class VoteUp {
    static readonly type = "[POST] Vote up";
    constructor(public id: string, public parentId: string) {}
  }

  export class VoteDown {
    static readonly type = "[POST] Vote down";
    constructor(public id: string, public parentId: string) {}
  }
}
