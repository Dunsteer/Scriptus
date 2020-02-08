import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "src/app/_core/components/base.component";
import { Store, Select } from "@ngxs/store";
import { Router, ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";

import { PostStateManager, PostState } from "@store/post/state";
import { Post } from "@models/post.model";
import { PostActions } from "@store/post/actions";

@Component({
  selector: "app-search",
  templateUrl: "./search.component.html",
  styleUrls: ["./search.component.scss"]
})
export class SearchComponent extends BaseComponent implements OnInit {
  terms: string;
  found = true;
  posts: Post[] = [];

  constructor(
    public _store: Store,
    public _router: Router,
    public _route: ActivatedRoute
  ) {
    super();
  }

  ngOnInit() {
    this._route.paramMap.subscribe(params => {
      this.terms = params.get("terms");
      const tags = this.terms.split("+");
      this._store.dispatch(new PostActions.Search(tags)).subscribe(
        (state: { post: PostState }) => {
          this.posts = state.post.posts;
        },
        err => {
          this.posts = [];
        }
      );

      (document.querySelector(
        "#searchInput"
      ) as HTMLInputElement).value = this.terms;
    });
  }
}
