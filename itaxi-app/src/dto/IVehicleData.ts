import { VehicleAvailability } from "../utilities/enums"

interface IVehicleData {

    driverId: string,
    vehicleTypeId: string,
    vehicleTypeDTO: {
        vehicleTypeName: string
    }
    vehicleMarkId: string,
    vehicleMarkDTO: {
        vehicleMarkName: string
    }
    vehicleModelId: string,
    vehicleModelDTO: {
        vehicleMarkName: string
    }
    vehiclePlateNumber: string,
    manufactureYear: number
    numberOfSeats: number,
    vehicleIdentifier: string,
    vehicleAvailability: Number,
    /* createdBy: string,
    createdAt: Date,
    updatedBy: string,
    updatedAt: Date, */

}

export type { IVehicleData }