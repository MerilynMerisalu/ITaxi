import { redirect } from "react-router-dom";
import { IDrive } from "../domain/IDrive";
import { BaseEntityService } from "./BaseEntityService";
import { IdentityService } from "./IdentityService";

 export class DriveService extends BaseEntityService<IDrive> {
    constructor(){
        super('v1/driverArea/drives');
    }  
    async getPdf(): Promise<string | undefined> {

        try {
          let user = IdentityService.getCurrentUser();
          let language = IdentityService.getLanguage();
          
          // if (user) {
            const response = await this.axios.get<string>('',
              {
                headers: {
                  'Authorization': 'Bearer ' + user?.token,
                  'Accept-Language': language
                }
              });
    
            console.log('response', response);
            if (response.status === 200) {
              redirect(response.data)
    
            } else {
              throw Error("User is not logged in");
            }
    
          // }
          // else {
          //   throw Error("User is not logged in");
          // }
    
    
          //return undefined;
    
        } catch (e) {
          console.log('error: ', (e as Error).message);
          return undefined;
        }
    
      }
}
             
