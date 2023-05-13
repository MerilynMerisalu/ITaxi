import { VehicleAvailability } from "../utilities/enums";
import { IBaseEntity } from "./Base/IBaseEntity";

export interface IVehicle extends IBaseEntity {
    driverId: string,
    vehicleTypeId: string,
    vehicleMarkId: string,
    vehicleModelId: string,
    vehiclePlateNumber: string,
    manufactureYear: number,        
    numberOfSeats: number,          
    VehicleIdentifier: string,
    vehicleAvailability: number,
    
    
}