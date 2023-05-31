import { IBaseEntity } from "./Base/IBaseEntity"

export interface IRideTime extends IBaseEntity {
    scheduleId: string
    schedule: {
        startDateAndTime: string,
        endDateAndTime: string,
        shiftDurationTime: string
    },
    
    rideDateTime: string,
    isTaken: boolean   
}
