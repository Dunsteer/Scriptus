import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "src/app/_core/components/base.component";
import { Store, Select } from "@ngxs/store";
import { Router, ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";

import { PostStateManager, PostState } from "@store/post/state";
import { Post } from "@models/post.model";
import { PostActions } from "@store/post/actions";
import { AuthStateManager } from "@store/auth/state";
import { User } from "@models/user.model";
import { FormGroup, FormControl, NgForm } from "@angular/forms";
import { ePostType } from "src/app/_core/enumerators/post-type.enum";

@Component({
  selector: "app-post",
  templateUrl: "./post.component.html",
  styleUrls: ["./post.component.scss"]
})
export class PostComponent extends BaseComponent implements OnInit {
  @Select(PostStateManager.post) post$: Observable<Post>;
  id: string;
  found = true;
  selectedQuestionNumber: number = -1;
  changed = true;

  commentForm: FormGroup;

  get text() {
    return this.commentForm.get("text");
  }

  get images() {
    return this.commentForm.get("images");
  }

  get answerFor() {
    return this.commentForm.get("answerFor");
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

    this.clearSearch();

    this.commentForm = new FormGroup({
      text: new FormControl(""),
      images: new FormControl(null),
      answerFor: new FormControl(null)
    });
  }

  createRange(number) {
    var items: number[] = [];
    for (var i = 1; i <= number; i++) {
      items.push(i);
    }
    return items;
  }

  submit(e, form: NgForm) {
    const post = this.commentForm.value as Post;
    post.type = ePostType.comment;
    this._store.dispatch(new PostActions.AddComment(this.id, post)).subscribe(
      (state: { post: PostState }) => {},
      err => {
        console.error(err);
      }
    );
  }

  filterComments(comments: Post[]) {
    console.log(comments);
    return comments.filter(comment => {
      console.log(comment);
      return (
        comment.answerFor == this.selectedQuestionNumber ||
        this.selectedQuestionNumber == -1
      );
    });
  }

  voteUp(id: string, parentId?: string) {
    if (parentId) {
      this._store
        .dispatch(new PostActions.VoteUpComment(id, parentId))
        .subscribe(this.refresh);
    } else {
      this._store.dispatch(new PostActions.VoteUp(id)).subscribe(this.refresh);
    }
  }

  voteDown(id: string, parentId?: string) {
    if (parentId) {
      this._store
        .dispatch(new PostActions.VoteDownComment(id, parentId))
        .subscribe(this.refresh);
    } else {
      this._store
        .dispatch(new PostActions.VoteDown(id))
        .subscribe(this.refresh);
    }
  }

  refresh() {
    this.changed = !this.changed;
  }
  test(e) {
    console.log(e);
  }

  remove(id: string) {
    this._store.dispatch(new PostActions.Remove(id));
  }

  openUrl(e: MouseEvent, url: string) {
    if (e.button == 0) {
      e.preventDefault();
      window.open(url, "_blank");
    }
  }

  onFilesSelect(event) {
    if (event.target.files.length > 0) {
      this._store
        .dispatch(new PostActions.FilesUpload(event.target.files))
        .subscribe((state: { post: PostState }) => {
          if (state.post.uploadedImages.length > 0) {
            this.images.setValue(state.post.uploadedImages);
          }
        });
    }
  }
}
