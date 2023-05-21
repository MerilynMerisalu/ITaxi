import { IRegisterData } from "./IRegisterData";

interface IJwtDriverRegisterResponse extends IRegisterData {
    token: string;
    refreshToken: string;
    driverDTO: {
        PersonalIdentifier?: string,
        DriverLicenseNumber: string,
        DriverLicenseCategoryIds: string[],
        DriverLicenseExpiryDate: string,
        CityId: string,
        Address: string

    }
 
}

export type {IJwtDriverRegisterResponse};