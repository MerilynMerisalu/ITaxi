import { IJwtLoginResponse } from "./IJwtLoginResponse";
import { IRegisterData } from "./IRegisterData";

interface IJwtCustomerRegisterResponse extends IRegisterData {
    token: string;
    refreshToken: string;
    customerDTO: {

        DisabilityTypeId: string,

    }
 
}
 
export type {IJwtCustomerRegisterResponse};