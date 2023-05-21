import { ICity } from "../domain/ICity";

import { BaseEntityService } from "./BaseEntityService";

 export class VehicleTypeService extends BaseEntityService<ICity> {
    constructor(){
        super('v1/adminArea/cities');
    }  

} 
