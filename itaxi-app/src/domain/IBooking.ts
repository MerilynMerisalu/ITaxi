import { IBaseEntity } from "./Base/IBaseEntity";

export interface IBooking extends IBaseEntity {
    
        driver: {
            appUser: {
                
                lastAndFirstName: string
            },
            
    }
    vehicleType: {
        vehicleTypeName: string
    },
    vehicle: {
        
        vehicleIdentifier: string
    },

    city: {
        cityName: string
    },
    pickUpDateAndTime:string,
    pickupAddress: string,
    destinationAddress: string,
    numberOfPassengers: string,
    hasAnAssistant: boolean,
    additionalInfo: string,
    statusOfBooking: number

}

