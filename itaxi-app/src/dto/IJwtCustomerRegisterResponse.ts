import { IRegisterData } from "./IRegisterData";

interface IJwtCustomerRegisterResponse extends IRegisterData {
    jwt: string;
    refreshToken: string;
    customerDTO: {

        DisabilityTypeId: string,

    }
 
}
export type {IJwtCustomerRegisterResponse};