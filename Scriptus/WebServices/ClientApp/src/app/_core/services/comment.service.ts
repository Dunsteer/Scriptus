import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { User } from "@models/user.model";
import { environment } from "src/environments/environment";
import { Observable, of } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class CommentService {
  constructor(private _http: HttpClient) {}

  fetch(id: string, page: number): Observable<Comment[]> {
    return this._http.get<Comment[]>(
      `${environment.serverUrl}/api/comments/${id}/${page}`
    );
  }

  fetchReplies(id: string): Observable<Comment[]> {
    return this._http.get<Comment[]>(
      `${environment.serverUrl}/api/comments/${id}/replies`
    );
  }

  addReply(id: string, data: string): Observable<Comment> {
    return this._http.post<Comment>(
      `${environment.serverUrl}/api/comments/${id}/reply`,
      data
    );
  }
}
