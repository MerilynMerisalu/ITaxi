import { VehicleAvailability } from "../utilities/enums";
import { IBaseEntity } from "./Base/IBaseEntity";

export interface IVehicle extends IBaseEntity {
    driverId: string,
    vehicleTypeId: string,
    vehicleMarkId: string,
    vehicleModelId: string,
    vehiclePlateNumber: string,
    manufactureYear: number,        // Number or string?
    numberOfSeats: number,          // Number or string?
    VehicleIdentifier: string,
    vehicleAvailability: number,
    //Schedules?
    //VehiclePhotos?
}