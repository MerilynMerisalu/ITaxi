import { IVehicleType } from "../domain/IVehicleType";

import { BaseEntityService } from "./BaseEntityService";

 export class VehicleTypeService extends BaseEntityService<IVehicleType> {
    constructor(){
        super('v1/adminArea/vehicletypes');
    }  

} 

    

    