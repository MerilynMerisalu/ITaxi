import { IDriverLicenseCategory } from "../domain/IDriverLicenseCategory";
import { BaseEntityService } from "./BaseEntityService";

 export class DriverLicenseCategoryService extends BaseEntityService<IDriverLicenseCategory> {
    constructor(){
        super('v1/adminArea/driverlicensecategories');
    }  

} 
