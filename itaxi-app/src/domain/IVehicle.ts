import { IBaseEntity } from "./Base/IBaseEntity";

export interface IVehicle extends IBaseEntity {
    DriverId: string,
    VehicleTypeId: string,
    VehicleMarkId: string,
    VehicleModelId: string,
    VehiclePlateNumber: string,
    ManufactureYear: string,        // Number or string?
    NumberOfSeats: string,          // Number or string?
    VehicleIdentifier: string,
    VehicleAvailability: string,
    //Schedules?
    //VehiclePhotos?
}