import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { User } from "@models/user.model";
import { environment } from "src/environments/environment";
import { Observable, of } from "rxjs";
import { Post } from "@models/post.model";
import { Parser } from "./parser.service";

@Injectable({
  providedIn: "root"
})
export class PostService {
  constructor(private _http: HttpClient, private _parser: Parser) {}

  fetch(id: string): Observable<Post> {
    return this._http.get<Post>(`${environment.serverUrl}/api/posts/${id}`);
  }

  search(tags: string[]): Observable<Post> {
    return this._http.get<Post>(
      `${environment.serverUrl}/api/posts`,
      this._parser.objectToUrlParams(tags)
    );
  }

  addComment(id: string, data: Post): Observable<Comment> {
    return this._http.post<Comment>(
      `${environment.serverUrl}/api/posts/${id}/comment/`,
      this._parser.objectToUrlParams(data)
    );
  }

  voteUp(id: string): Observable<number> {
    return this._http.post<number>(
      `${environment.serverUrl}/api/posts/${id}/vote-up/`,
      null
    );
  }

  voteDown(id: string): Observable<number> {
    return this._http.post<number>(
      `${environment.serverUrl}/api/posts/${id}/vote-down/`,
      null
    );
  }
}
