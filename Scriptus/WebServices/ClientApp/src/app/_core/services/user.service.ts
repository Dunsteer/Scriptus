import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { User } from "@models/user.model";
import { environment } from "src/environments/environment";
import { Observable, of } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class UserService {
  constructor(private _http: HttpClient) {}

  fetch(id: string): Observable<User> {
    return this._http.get<User>(`${environment.serverUrl}/api/users/${id}`);
  }

  fetchTop(from: number, to: number): Observable<User[]> {
    return this._http.get<User[]>(
      `${environment.serverUrl}/api/users/top/${from}/${to}`
    );
  }
}
