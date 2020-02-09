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

  examPlaceholderTemplate = "npr. tIspit - septembar 2017.";
  scriptPlaceholderTemplate = "npr. tSkripta - 02.10.2019.";
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
      name: new FormControl("", Validators.required),
      tags: new FormControl("", Validators.required),
      text: new FormControl(""),
      images: new FormControl(null),
      numberOfQuestions: new FormControl(1),
      pdf: new FormControl(null)
    });

    this.clearSearch();
  }

  typeChanged(type: string) {
    const eType = parseInt(type) as ePostType;
    this.images.setValue(null);
    this.images.reset();
    this.pdf.setValue(null);
    this.pdf.reset();
    switch (eType) {
      case ePostType.exam:
        {
          this.images.setValidators(Validators.required);
          this.pdf.setValidators([]);
          this.namePlaceholder = this.examPlaceholderTemplate;
          this.images.updateValueAndValidity();
          this.pdf.updateValueAndValidity();
        }
        break;
      case ePostType.script:
        {
          this.images.setValidators([]);
          this.pdf.setValidators(Validators.required);
          this.namePlaceholder = this.scriptPlaceholderTemplate;
          this.images.updateValueAndValidity();
          this.pdf.updateValueAndValidity();
        }
        break;
    }
  }

  submit(e, form: NgForm) {
    const post = this.createForm.value;
    post.tags = (post.tags as string).split(",").map(x => x.trim());
    this._store.dispatch(new PostActions.Create(post)).subscribe(
      (state: { post: PostState }) => {
        this._router.navigateByUrl(`/post/${state.post.post.id}`);
      },
      err => {
        console.error(err);
      }
    );
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

  onFileSelect(event) {
    if (event.target.files.length > 0) {
      this._store
        .dispatch(new PostActions.FileUpload(event.target.files[0]))
        .subscribe((state: { post: PostState }) => {
          if (state.post.uploadedPdf) {
            this.pdf.setValue(state.post.uploadedPdf);
          }
        });
    }
  }
}
