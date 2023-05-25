import { IDrive } from "../domain/IDrive";
import { BaseEntityService } from "./BaseEntityService";

 export class DriveService extends BaseEntityService<IDrive> {
    constructor(){
        super('v1/driverArea/drives');
    }  
} 
