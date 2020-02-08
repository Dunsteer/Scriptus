import { Component, OnInit } from "@angular/core";
import { BaseComponent } from "src/app/_core/components/base.component";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html"
})
export class HomeComponent extends BaseComponent implements OnInit {
  ngOnInit(): void {
    this.clearSearch();
  }
}
