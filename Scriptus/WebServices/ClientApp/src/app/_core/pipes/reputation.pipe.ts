import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'reputation' })
export class ReputationPipe implements PipeTransform {
  transform(value: number): string {
    let res;
    if (value < 10) {
      res = "regular";
    }

    if (value >= 10 && value < 20) {
      res = "poznanik"
    }

    if (value >= 20) {
      res = "VIP"
    }

    return res;
  }
}
