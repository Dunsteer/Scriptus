import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { User } from "@models/user.model";
import { environment } from "src/environments/environment";
import { Observable, of } from "rxjs";
import { Post } from "@models/post.model";
import { Comment } from "@models/comment.model";

@Injectable({
  providedIn: "root"
})
export class PostService {
  constructor(private _http: HttpClient) {}

  fetch(id: string): Observable<Post> {
    return this._http.get<Post>(`${environment.serverUrl}/api/posts/${id}`);
  }

  addComment(id: string, data: string): Observable<Comment> {
    return this._http.post<Comment>(
      `${environment.serverUrl}/api/posts/${id}/comment/`,
      data
    );
  }
}
