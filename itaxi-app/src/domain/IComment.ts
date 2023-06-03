import { IBaseEntity } from "./Base/IBaseEntity";

export interface IComment extends IBaseEntity {
        driveCustomerStr: string,
        driveDescription?: string
        driverName: string
        commentText: string
        driveTimeAndDriver: string
    }
    
    

