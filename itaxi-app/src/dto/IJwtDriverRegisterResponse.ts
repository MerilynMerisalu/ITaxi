import { IRegisterData } from "./IRegisterData";

interface IJwtDriverRegisterResponse extends IRegisterData {
    token: string;
    refreshToken: string;
    driverDTO: {
        
        PersonalIdentifier?: string,
        DriverLicenseNumber: string,
        DriverLicenseCategories: string[],
        DriverLicenseExpiryDate: string,
        CityId: string,
        Address: string

    }
 
}

export type {IJwtDriverRegisterResponse};