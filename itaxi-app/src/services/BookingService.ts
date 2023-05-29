import { IBooking } from "../domain/IBooking";

import { BaseEntityService } from "./BaseEntityService";
import { IdentityService } from "./IdentityService";

export class BookingService extends BaseEntityService<IBooking> {
  constructor() {
    super('v1/customerarea/bookings');
  }

  getBookingStatus(statusOfBooking: number): string | undefined {
    switch (statusOfBooking) {
      case 1: return "Awaiting for Confirmation";
      case 2: return "Accepted";
      case 3: return "Declined";
      default: return "Awaiting";
    }
  }
  

  async details(date?: string): Promise<IBooking | undefined> {
    try {
      let user = IdentityService.getCurrentUser();
      let language = IdentityService.getLanguage();
      if (user) {
        let response = await this.axios.get(`/${date}`,
          {
            headers: {
              'Authorization': 'Bearer ' + user.token,
              'Accept-Language': language
            }
          });
        if (response.status === 200) {
          return response.data;
        }
      }
      else {
        throw Error("User is not logged in");
      }
      return undefined;
    } catch (e) {
      console.log('Details -  error: ', (e as Error).message);
      return undefined;
    }
  }

  async declineDetails(id?: string): Promise<IBooking | undefined> {
    try {
      let user = IdentityService.getCurrentUser();
      if (user) {
        let response = await this.axios.get(`/${id}`,
          {
            headers: {
              'Authorization': 'Bearer ' + user.token
            }
          });
        if (response.status === 200) {
          return response.data;
        }
      }
      else {
        throw Error("User is not logged in");
      }
      return undefined;
    } catch (e) {
      console.log('Details -  error: ', (e as Error).message);
      return undefined;
    }
  }

  async decline(id?: string): Promise<number | undefined> {
    console.log('id', id)
    try {
      let user = IdentityService.getCurrentUser();
      if (user) {
        console.log('this.axios', this.axios.defaults.baseURL)
        let response = await this.axios.put(`/${id}`,
          {
            headers: {
              'Authorization': 'Bearer ' + user.token
            }
          });
        console.log('response.status:', response.status)
        if (response.status === 204) {
          return response.status
        }
      }
      else {
        throw Error("User is not logged in");
      }
      return undefined;
    } catch (e) {
      console.log('Details -  error: ', (e as Error).message);
      return undefined;
    }
  }
}
