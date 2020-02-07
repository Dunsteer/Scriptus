import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root"
})
export class Parser {
  objectToUrlParams(obj) {
    const urlParams = {};
    for (const x in obj) {
      if (obj[x] != undefined) {
        if (obj[x] instanceof Date) {
          urlParams[x.charAt(0).toUpperCase() + x.slice(1)] = obj[
            x
          ].toISOString();
        } else {
          urlParams[x.charAt(0).toUpperCase() + x.slice(1)] = obj[x];
        }
      }
    }

    return urlParams;
  }
}
