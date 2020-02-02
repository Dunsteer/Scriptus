import { User } from "@models/user.model";
import { Comment } from "@models/comment.model";

export namespace CommentActions {
  export class Fetch {
    static readonly type = "[COMMENT] Fetch";
    constructor(public id: string) {} // post id
  }

  export class FetchReplies {
    static readonly type = "[COMMENT] Fetch replies";
    constructor(public id: string) {} // comment id
  }

  export class AddReply {
    static readonly type = "[COMMENT] Add reply";
    constructor(public id: string, public data: string) {}
  }
}
