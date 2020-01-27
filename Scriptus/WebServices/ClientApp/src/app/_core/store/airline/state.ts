import { State, Action, StateContext, Selector } from "@ngxs/store";
import { AirlineService } from "@services/airline.service";
import { AirlineActions } from "./actions";
import { map, catchError } from "rxjs/operators";
import { of } from "rxjs";
import { Company } from "@models/company.model";
import { Flight } from "@models/flight.model";
import {
  patch,
  append,
  removeItem,
  insertItem,
  updateItem
} from "@ngxs/store/operators";
import { Reservation } from "@models/reservation.model";
import { AuthActions } from "@store/auth/actions";
import * as neo4j from "neo4j-driver";

export interface AirlineState {
  companies: Company[];
}

const initialState: AirlineState = {
  companies: []
};

@State<AirlineState>({ name: "airline", defaults: initialState })
export class AirlineStateManager {
  constructor(private airline: AirlineService) { }

  @Action(AirlineActions.GetCompanies)
  getCompanies(
    ctx: StateContext<AirlineState>,
    action: AirlineActions.GetCompanies
  ) {
    const state = ctx.getState();
    return this.airline.getCompanies(action.filters).pipe(
      map(res => {
        if (res) {
          return ctx.setState({
            ...state,
            companies: this.mapN4J(res)
          });
        }
      }),
      catchError(err => {
        console.error(err);
        return of(err);
      })
    );
  }

  @Action(AirlineActions.Reserve)
  reserve(ctx: StateContext<AirlineState>, action: AirlineActions.Reserve) {
    return this.airline.reserve(action.seatsId).pipe(
      map(res => {
        if (res) {
          ctx.dispatch(new AuthActions.AddReservation(res));
        }
      }),
      catchError(err => {
        console.error(err);
        return of(err);
      })
    );
  }

  @Selector()
  static state(state: AirlineState) {
    return state;
  }

  @Selector()
  static companies(state: AirlineState) {
    return state.companies;
  }

  mapN4J(input) {
    function walker(obj) {
      const keys = Object.keys(obj);

      keys.forEach(k => {
        if (typeof (obj[k]) == 'object') {
          if (neo4j.isInt(obj[k])) {
            obj[k] = neo4j.integer.toNumber(obj[k]);

            return;
          }

          if (neo4j.isDateTime(obj[k])) {
            obj[k] = new Date(obj[k].year,
              obj[k].month,
              obj[k].day,
              obj[k].hour,
              obj[k].minute,
              obj[k].second)

            return;
          }

          if (Array.isArray(obj[k])) {
            obj[k] = obj[k].map(x => {
              return walker(x);
            })

            return;
          }

          return walker(obj[k]);

        }
      });

      return obj;
    }

    console.log(input);
    const output = walker(input);
    console.log(output);
    return output;
  }
}
