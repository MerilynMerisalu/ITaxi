import { IBaseEntity } from "./Base/IBaseEntity";

export interface IComment extends IBaseEntity {
    drive: {
        booking: {
            pickUpDateAndTime: string
        }
    },
    
    
}