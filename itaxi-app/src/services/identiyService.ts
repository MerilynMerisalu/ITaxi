import { IJwtCustomerRegisterResponse } from "../dto/IJwtCustomerRegisterResponse";
import { IRegisterCustomerData } from "../dto/IRegisterCustomerData";
import BaseService from "./BaseService";

export class IdentityService extends BaseService {
    constructor(){
        super('v1/identity/account/');
    }
    
    async registerCustomer(data: IRegisterCustomerData): Promise<IJwtCustomerRegisterResponse | undefined> {
        try {
            const response = await this.axios.post<IJwtCustomerRegisterResponse>('RegisterCustomerDTO', data);

            console.log('RegisterCustomerDTO response', response);
            if (response.status === 200) {
                return response.data;
            }
            return undefined;

        } catch (e){
            console.log('error: ', (e as Error).message);
            return undefined;
        }

    }

}