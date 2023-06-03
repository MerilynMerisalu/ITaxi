import { redirect } from "react-router-dom";
import { IDrive } from "../domain/IDrive";
import { BaseEntityService } from "./BaseEntityService";
import { IdentityService } from "./IdentityService";

 export class DriveService extends BaseEntityService<IDrive> {
    constructor(){
        super('v1/driverArea/drives');
    }
    
    getDriveStatus(statusOfDrive: number): string | undefined {
      console.log(statusOfDrive)
        switch (statusOfDrive) {
          case 1: return "Awaiting for Confirmation";
          case 2: return "Accepted";
          case 3: return "Declined";
          case 4: return "InProgress";
          case 5: return "Finished";
          default: return "Awaiting";
        }
      }
    
    async acceptDetails(id?: string): Promise<IDrive | undefined> {
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

      async accept(id?: string): Promise<number | undefined> {
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
             
