import { IVehicleMark } from "../domain/IVehicleMark";
import { BaseEntityService } from "./BaseEntityService";

 export class VehicleMarkService extends BaseEntityService<IVehicleMark> {
    constructor(){
        super('v1/adminArea/vehiclemarks');
    }  

} 
