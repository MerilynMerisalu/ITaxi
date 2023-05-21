import { IRegisterData } from "./IRegisterData";

interface IRegisterDriverData extends IRegisterData {
    CityId: string,

    DriverLicenseCategoryId: string,
    DriverLicenseCategoryIds: string[],
    PersonalIdentifier?: string,
    Address: string
    DriverLicenseNumber: string,
    DriverLicenseExpiryDate: string
    
    }


   /*  DriverAndLicenseCategories: {
        DriverLicenseCategoryIds: string[],
    }[], */

    

export type {IRegisterDriverData}