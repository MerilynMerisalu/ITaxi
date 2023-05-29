import { IRideTime } from "../domain/IRideTime";

import { BaseEntityService } from "./BaseEntityService";

 export class RideTimeService extends BaseEntityService<IRideTime> {
    constructor(){
        super('v1/driverarea/rideTimes');
    }  

} 
