import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "src/app/_core/components/base.component";
import { Store, Select } from "@ngxs/store";
import { Router, ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";

import { PostStateManager, PostState } from "@store/post/state";
import { Post } from "@models/post.model";
import { PostActions } from "@store/post/actions";

@Component({
  selector: "app-post",
  templateUrl: "./post.component.html",
  styleUrls: ["./post.component.scss"]
})
export class PostComponent extends BaseComponent implements OnInit {
  @Select(PostStateManager.post) post$: Observable<Post>;
  id: string;
  found = true;
  selectedQuestionNumber: number = null;

  reputation(post: Post): number {
    return post.voteUps.length - post.voteDown.length;
  }

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
        (post: { post: PostState }) => {
          this.found = true;
        },
        err => {
          this.found = false;
        }
      );
    });
  }

  createRange(number){
    var items: number[] = [];
    for(var i = 1; i <= number; i++){
       items.push(i);
    }
    return items;
  }

  addComment(data: string) {
    this._store.dispatch(new PostActions.AddComment(this.id, data));
  }

  filterComments(comments: Post[]) {
    return comments.filter(
      comment =>
        comment.answerFor == this.selectedQuestionNumber ||
        this.selectedQuestionNumber == null
    );
  }
}
