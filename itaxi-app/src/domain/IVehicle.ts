import { IBaseEntity } from "./Base/IBaseEntity";
import { VehicleAvailability } from "../utilities/enums";

export interface IVehicle extends IBaseEntity {
    driverId: string,
    vehicleTypeId: string,
    vehicleType: {
        vehicleTypeName: string
    }
    
    vehicleMarkId: string,
    vehicleMark: {
        vehicleMarkName: string
    }
    vehicleModelId: string,
    vehicleModel: {
        vehicleModelName: string
    }
    vehiclePlateNumber: string,
    manufactureYear: number
    numberOfSeats: number,
    vehicleIdentifier: string,
    vehicleAvailability: Number
    createdBy: string,
    createdAt: Date,
    updatedBy: string,
    updatedAt: Date
}