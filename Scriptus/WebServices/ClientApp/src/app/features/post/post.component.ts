import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "src/app/_core/components/base.component";
import { Store, Select } from "@ngxs/store";
import { Router, ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";

import { PostStateManager, PostState } from "@store/post/state";
import { Post } from "@models/post.model";
import { PostActions } from "@store/post/actions";
import { CommentActions } from "@store/comment/actions";
import { CommentStateManager } from "@store/comment/state";

@Component({
  selector: "app-post",
  templateUrl: "./post.component.html",
  styleUrls: ["./post.component.scss"]
})
export class PostComponent extends BaseComponent implements OnInit {
  @Select(PostStateManager.post) post$: Observable<Post>;
  @Select(CommentStateManager.comments) comments$: Observable<Comment[]>;
  id: string;
  found = true;

  constructor(
    public _store: Store,
    public _router: Router,
    public _route: ActivatedRoute
  ) {
    super();
  }

  ngOnInit() {
    this._route.paramMap.subscribe(params => {
      this.id = params.get("id");
      this._store.dispatch(new PostActions.Fetch(this.id)).subscribe(
        (state: PostState) => {
          this._store.dispatch(new CommentActions.Fetch(this.id));
          this.found = true;
        },
        err => {
          this.found = false;
        }
      );
    });
  }

  loadMoreComments() {
    this._store.dispatch(new CommentActions.Fetch(this.id));
  }

  loadReplies(id: string) {
    this._store.dispatch(new CommentActions.FetchReplies(id));
  }

  addComment(data: string) {
    this._store.dispatch(new PostActions.AddComment(this.id, data));
  }

  addReply(id: string, data: string) {
    this._store.dispatch(new CommentActions.AddReply(id, data));
  }
}
