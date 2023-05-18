import { IVehicleModel } from "../domain/IVehicleModel";
import { BaseEntityService } from "./BaseEntityService";

 export class VehicleModelService extends BaseEntityService<IVehicleModel> {
    constructor(){
        super('v1/adminArea/vehiclemodels');
    }  

} 
