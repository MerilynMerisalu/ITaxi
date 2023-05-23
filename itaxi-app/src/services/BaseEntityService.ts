import { useEffect, useState } from "react";
import { IBaseEntity } from "../domain/Base/IBaseEntity";
import { IdentityService } from "./IdentityService";
import BaseService from "./BaseService";
import { AnyCnameRecord } from "dns";
import { IVehicleFormData } from "../dto/IVehicleFormData";

export abstract class BaseEntityService<TEntity extends IBaseEntity> extends BaseService {
  constructor(baseUrl: string) {
    console.log('baseentityserivce constructor baseurl', baseUrl)
    super(baseUrl);
  }

  async getAll(): Promise<TEntity[] | undefined> {

    try {
      let user = IdentityService.getCurrentUser();
      let language = IdentityService.getLanguage();
      
      // if (user) {
        const response = await this.axios.get<TEntity[]>('',
          {
            headers: {
              'Authorization': 'Bearer ' + user?.token,
              'Accept-Language': language
            }
          });

        console.log('response', response);
        if (response.status === 200) {
          return response.data;

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


  async details(id?: string): Promise<TEntity | undefined> {
    try {
      let user = IdentityService.getCurrentUser();
      let language = IdentityService.getLanguage();
      if (user) {
        let response = await this.axios.get(`/${id}`,
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


  async deleteDetails(id?: string): Promise<TEntity | undefined> {
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

  async delete(id?: string): Promise<number | undefined> {
    console.log('id', id)
    try {
      let user = IdentityService.getCurrentUser();
      if (user) {
        console.log('this.axios', this.axios.defaults.baseURL)
        let response = await this.axios.delete(`/${id}`,
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

  async create(body: IVehicleFormData): Promise<number | undefined> {
    console.log('body', body)
    try {
      let user = IdentityService.getCurrentUser();
      if (user) {
        console.log('this.axios', this.axios.defaults.baseURL)
        let response = await this.axios.post(`/`, body,
          {
            headers: {
              'Authorization': 'Bearer ' + user.token
            }
          });
        console.log('response.status:', response.status)
        if (response.status === 201) {
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
  async edit(id: string, body: IVehicleFormData): Promise<number | undefined> {
    console.log('body', body)
    try {
      let user = IdentityService.getCurrentUser();
      if (user) {
        console.log('this.axios', this.axios.defaults.baseURL)
        let response = await this.axios.put(`/${id}`, body,
          {
            headers: {
              'Authorization': 'Bearer ' + user.token
            }
          });
        console.log('response.status:', response.status)
        if (response.status === 201) {
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