import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { RegisterComponent } from "./features/register/register.component";
import { LoginComponent } from "./features/login/login.component";
import { HomeComponent } from "./features/home/home.component";
import { UserComponent } from "./features/user/user.component";
import { PostComponent } from "./features/post/post.component";
import { CreateComponent } from "./features/create/create.component";
import { SearchComponent } from "./features/search/search.component";

const routes: Routes = [
  //{ path: "register", component: RegisterComponent },
  //{ path: "login", component: LoginComponent },
  { path: "user", component: UserComponent },
  { path: "user/:id", component: UserComponent },
  { path: "post/:id", component: PostComponent },
  { path: "create", component: CreateComponent },
  { path: "search/:terms", component: SearchComponent },
  { path: "", component: HomeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
