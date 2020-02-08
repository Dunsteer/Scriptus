import { Component } from "@angular/core";
import { BaseComponent } from "src/app/_core/components/base.component";
import { Router } from "@angular/router";

@Component({
  selector: "app-nav-menu",
  templateUrl: "./nav-menu.component.html",
  styleUrls: ["./nav-menu.component.scss"]
})
export class NavMenuComponent extends BaseComponent {
  constructor(private _router: Router) {
    super();
  }
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
  search(text: string) {
    text = text.replace(' ', "+");
    this._router.navigateByUrl(`/search/${text}`);
  }
}
