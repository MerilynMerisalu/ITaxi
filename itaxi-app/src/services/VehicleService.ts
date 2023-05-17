import { IVehicle } from "../domain/IVehicle";
import { BaseEntityService } from "./BaseEntityService";
import { IdentityService } from "./IdentityService";

export class VehicleService extends BaseEntityService<IVehicle> {
    constructor() {
        super('v1/driverarea/vehicles')
    }

    async gallery(id?: string): Promise<IVehicle | undefined> {
        try {
          let user = IdentityService.getCurrentUser();
          if(user)
          {
          let response = await this.axios.get(`/${id}`, 
          {
            headers: {
              'Authorization': 'Bearer ' + user.token
            }
          });
          if (response.status === 200) {
            console.log(response);
            return response.data;
          }
        }
        else{
          throw Error("User is not logged in"); 
        }
          return undefined;
        } catch (e) {
          console.log('Details -  error: ', (e as Error).message);
          return undefined;
        }
      }

      
      async getManufactureYears (): Promise<number[] | undefined> {
        try {
          let user = IdentityService.getCurrentUser();
          if(user)
          {
          let response = await this.axios.get(`/GetManufactureYears`,
          {
            headers: {
              'Authorization': 'Bearer ' + user.token
            }
          });
          if (response.status === 200) {
            console.log(response);
            return response.data;
          }
        }
        else{
          throw Error("User is not logged in"); 
        }
          return undefined;
        } catch (e) {
          console.log('Details -  error: ', (e as Error).message);
          return undefined;
        }
     }
}