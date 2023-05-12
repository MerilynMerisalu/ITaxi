import { IVehicle } from "../domain/IVehicle";
import { BaseEntityService } from "./BaseEntityService";

export class VehicleService extends BaseEntityService<IVehicle> {
    constructor(){
        super('v1/driverarea/vehicles');
    }
}