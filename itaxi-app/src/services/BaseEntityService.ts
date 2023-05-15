import { useEffect } from "react";
import { IBaseEntity } from "../domain/Base/IBaseEntity";
import { IdentityService } from "./IdentityService";
import BaseService from "./BaseService";

export abstract class BaseEntityService <TEntity extends IBaseEntity> extends BaseService {
  constructor(baseUrl: string){
      super(baseUrl);
  }

  async getAll(): Promise<TEntity[] | undefined > {

    try {
      var user = IdentityService.getCurrentUser();
      if(user) {
      const response = await this.axios.get<TEntity[]>('', 
        {
          headers: {
            'Authorization': 'Bearer ' + user.token
          }
        });

        console.log('response', response);
        if (response.status === 200) {
          return response.data;
          
      }
  
      }
      else{
        throw Error("User is not logged in"); 
      }

      
      return undefined;

  } catch (e){
      console.log('error: ', (e as Error).message);
      return undefined;
  }
    
  }
 
  
  async details(id?: string): Promise<TEntity | undefined> {
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