import { useEffect } from "react";
import { IBaseEntity } from "../domain/Base/IBaseEntity";
import BaseService from "./BaseService";

export abstract class BaseEntityService <TEntity extends IBaseEntity> extends BaseService {
  constructor(baseUrl: string){
      super(baseUrl);
  }

  async getAll(token: string): Promise<TEntity[] | undefined> {

    try {
      
      const response = await this.axios.get<TEntity[]>('', 
      /*
        {
          headers: {
            'Authorization': 'Bearer ' + token
          }

        }
        */

        
      );

      

      console.log('response', response);
      if (response.status === 200) {
          return response.data;
          
      }
      return undefined;

  } catch (e){
      console.log('error: ', (e as Error).message);
      return undefined;
  }
    
  }
 
  
  async details(id?: string, jwt?: string): Promise<TEntity | undefined> {
    try {
      let response = await this.axios.get(`/${id}`);
      if (response.status === 200) {
        return response.data;
      }
      return undefined;
    } catch (e) {
      console.log('Details -  error: ', (e as Error).message);
      return undefined;
    }
  }
}