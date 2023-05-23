import { IJwtLoginResponse } from "./IJwtLoginResponse";
import { IRegisterData } from "./IRegisterData";

interface IJwtCustomerRegisterResponse2 extends IRegisterData {
    token: string;
    refreshToken: string;
    customerDTO: {

        DisabilityTypeId: string,

    }
 
}
type IJwtCustomerRegisterResponse = IJwtCustomerRegisterResponse2 & IJwtLoginResponse
export type {IJwtCustomerRegisterResponse};