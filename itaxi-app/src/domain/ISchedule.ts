import { IBaseEntity } from "./Base/IBaseEntity"

export interface ISchedule extends IBaseEntity {
    vehicleId: string,
    vehicle: {
        vehicleIdentifier: string,
        vehicleModel: string,
        vehicleMarkName: string,
        vehicleTypeName: string,
        vehiclePlateNumber: string
    }
    startDateAndTime: string,         
    endDateAndTime: string           
}

