import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { User } from "@models/user.model";
import { environment } from "src/environments/environment";
import { Observable, of } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class AuthService {
  constructor(private _http: HttpClient) {}

  register(user: User): Observable<{ token: string }> {
    return this._http.post<{ token: string }>(
      `${environment.serverUrl}/api/users/register`,
      user
    );
  }

  login(user: User): Observable<{ user: User; token: string }> {
    return this._http.post<{ user: User; token: string }>(
      `${environment.serverUrl}/api/users/login`,
      user
    );
  }

  check(): Observable<User> {
    return this._http.get<User>(`${environment.serverUrl}/api/users/check`);
  }
}
