import { User } from '@models/user.model';
import { Company } from '@models/company.model';
import { Flight } from '@models/flight.model';
import { Reservation } from '@models/reservation.model';
import { SeatType } from '@models/seat-configuration.model';

export namespace AirlineActions {
  export class GetCompanies {
    static readonly type = "[AIRLINE] Get companies";
    constructor(public filters: Company) { }
  }

  export class Reserve {
    static readonly type = "[AIRLINE] Reserve";
    constructor(public seatsId: number) { }
  }
}
