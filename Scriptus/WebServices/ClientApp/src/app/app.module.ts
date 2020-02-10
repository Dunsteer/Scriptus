import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { RouterModule } from "@angular/router";

import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./features/nav-menu/nav-menu.component";
import { HomeComponent } from "./features/home/home.component";
import { InterceptorService } from "@services/interceptor.service";
import { CookieService } from "ngx-cookie-service";
import { AppRoutingModule } from "./app-routing.module";
import { RegisterComponent } from "./features/register/register.component";
import { LoginComponent } from "./features/login/login.component";
import { ReactiveFormsModule } from "@angular/forms";
import { NgxsModule } from "@ngxs/store";
import { SocketIoModule, SocketIoConfig } from "ngx-socket-io";
import { environment } from "src/environments/environment";
import { AppState } from "./app.state";
import { NgxsReduxDevtoolsPluginModule } from "@ngxs/devtools-plugin";
import { UserComponent } from "./features/user/user.component";
import { PostComponent } from "./features/post/post.component";
import { CreateComponent } from "./features/create/create.component";
import { SearchComponent } from "./features/search/search.component";
import { ReputationPipe } from "./_core/pipes/reputation.pipe";

const config: SocketIoConfig = { url: environment.serverUrl, options: {} };

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    RegisterComponent,
    LoginComponent,
    UserComponent,
    PostComponent,
    CreateComponent,
    SearchComponent,
    ReputationPipe
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: "ng-cli-universal" }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    NgxsModule.forRoot(AppState),
    NgxsReduxDevtoolsPluginModule.forRoot(),
    SocketIoModule.forRoot(config)
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: InterceptorService,
      multi: true,
      deps: [CookieService]
    },
    CookieService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
