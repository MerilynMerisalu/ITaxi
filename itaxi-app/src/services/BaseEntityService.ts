import axios from "axios";
import { IBaseEntity } from "../domain/Base/IBaseEntity";
import BaseService from "./BaseService";

export abstract class BaseEntityService<TEntity extends IBaseEntity> extends BaseService {
    constructor(baseUrl: string) {
        super(baseUrl);
    } 

    async getAll(): Promise<TEntity[]| null> {
        try {
            axios.get("https://localhost:7026/api/v1/adminarea/disabilityTypes", {
        headers: {
          "Content-Type": "application/json"
        }
      }).then(response => {
        console.log('*** RES', response)
         setDisabilityTypes(response.data);
      });

      const today = new Date().getDate()
console.log(today)
      console.log("Try get data");
      //getData();

    }, []);

        } catch (error) {
            
        }

    }
}