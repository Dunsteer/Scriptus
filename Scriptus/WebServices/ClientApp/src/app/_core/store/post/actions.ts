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
    constructor(public id: string,) {}
  }

  export class VoteDown {
    static readonly type = "[POST] Vote down";
    constructor(public id: string) {}
  }

  export class VoteUpComment {
    static readonly type = "[POST] Vote up comment";
    constructor(public id: string, public parentId: string) {}
  }

  export class VoteDownComment {
    static readonly type = "[POST] Vote down comment";
    constructor(public id: string, public parentId: string) {}
  }

  export class FilesUpload {
    static readonly type = "[POST] Files upload";
    constructor(public files: FileList) {}
  }

  export class FileUpload {
    static readonly type = "[POST] File upload";
    constructor(public file: File) {}
  }
}
