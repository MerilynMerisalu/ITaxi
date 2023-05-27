interface IScheduleData {

    vehicleId: string,
    vehicle: {
        vehicleIdentifier: string,
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
        vehicleModelName: string
    }
        
        vehiclePlateNumber: string
    }
    startDateAndTime: string,         
    endDateAndTime: string           

    /* 
    vehiclePlateNumber: string,
    manufactureYear: number
    numberOfSeats: number,
    vehicleIdentifier: string,
    vehicleAvailability: Number, */
    /* createdBy: string,
    createdAt: Date,
    updatedBy: string,
    updatedAt: Date, */

}

export type { IScheduleData }