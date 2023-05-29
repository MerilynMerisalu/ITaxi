import { IBaseEntity } from "./Base/IBaseEntity"

export interface IRideTime extends IBaseEntity {
    schedule: {
        startDateAndTime: string,
        endDateAndTime: string,
        shiftDurationTime: string
    },
    
    rideDateTime: string,
    isTaken: boolean   
}
