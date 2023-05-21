import { ICity } from "../domain/ICity";

import { BaseEntityService } from "./BaseEntityService";

 export class CityService extends BaseEntityService<ICity> {
    constructor(){
        super('v1/adminArea/cities');
    }  

} 
