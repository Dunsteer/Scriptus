import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "src/app/_core/components/base.component";
import { Store, Select } from "@ngxs/store";
import { Router, ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";

import { PostStateManager, PostState } from "@store/post/state";
import { Post } from "@models/post.model";
import { PostActions } from "@store/post/actions";
import { FormGroup, FormControl, Validators, NgForm } from "@angular/forms";
import { ePostType } from "src/app/_core/enumerators/post-type.enum";

@Component({
  selector: "app-create",
  templateUrl: "./create.component.html",
  styleUrls: ["./create.component.scss"]
})
export class CreateComponent extends BaseComponent implements OnInit {
  createForm: FormGroup;

  constructor(
    public _store: Store,
    public _router: Router,
    public _route: ActivatedRoute
  ) {
    super();
  }

  examPlaceholderTemplate = "Ispit - septembar 2017.";
  scriptPlaceholderTemplate = "Skripta - 02.10.2019.";
  namePlaceholder = this.examPlaceholderTemplate;

  get type() {
    return this.createForm.get("type");
  }

  get name() {
    return this.createForm.get("name");
  }

  get tags() {
    return this.createForm.get("tags");
  }

  get text() {
    return this.createForm.get("text");
  }

  get images() {
    return this.createForm.get("images");
  }

  get numberOfQuestions() {
    return this.createForm.get("numberOfQuestions");
  }

  get pdf() {
    return this.createForm.get("pdf");
  }

  get allowedTypes(): number[] {
    return [ePostType.exam, ePostType.script];
  }

  ngOnInit() {
    this.createForm = new FormGroup({
      type: new FormControl(-1),
      name: new FormControl(""),
      tags: new FormControl(""),
      text: new FormControl("", Validators.required),
      images: new FormControl(null),
      numberOfQuestions: new FormControl(1),
      pdf: new FormControl(null)
    });
  }

  typeChanged(type: string) {
    console.log(type);
    const eType = parseInt(type) as ePostType;
    switch (eType) {
      case ePostType.exam:
        {
          this.text.setValidators(Validators.required);
          this.pdf.clearValidators();
          this.namePlaceholder = this.examPlaceholderTemplate;
          this.pdf.updateValueAndValidity();
          this.text.updateValueAndValidity();
        }
        break;
      case ePostType.script:
        {
          this.pdf.setValidators(Validators.required);
          this.text.clearValidators();
          this.namePlaceholder = this.scriptPlaceholderTemplate;
          this.pdf.updateValueAndValidity();
          this.text.updateValueAndValidity();
        }
        break;
    }
  }

  submit(e, form: NgForm) {
    e.preventDefault();
    this._store
      .dispatch(new PostActions.Create(this.createForm.value))
      .subscribe(
        (state: { post: PostState }) => {
          this._router.navigateByUrl(`/post/${state.post.post.id}`);
        },
        err => {
          console.error(err);
        }
      );
  }
}
