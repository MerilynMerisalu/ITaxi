import axios from "axios";
import { IJwtCustomerRegisterResponse } from "../dto/IJwtCustomerRegisterResponse";
import { IJwtLoginResponse } from "../dto/IJwtLoginResponse";
import { ILoginData } from "../dto/ILoginData";
import { IRegisterCustomerData } from "../dto/IRegisterCustomerData";
import BaseService from "./BaseService";
import { useNavigate } from "react-router-dom";

export class IdentityService extends BaseService {
    constructor(){
        super('v1/identity/account/');
    }
    
    async registerCustomer(data: IRegisterCustomerData): Promise<IJwtCustomerRegisterResponse | undefined> {
        try {
            // Re-assert that Gender is numeric
            data.Gender = +data.Gender;
            console.log(typeof data.Gender)
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

    async login(data: ILoginData): Promise<IJwtLoginResponse | undefined> {
        try {
            const response = await this.axios.post<IJwtLoginResponse>('login', data);

            console.log('login response', response);
            if (response.status === 200) {

                // Add the token to the default axios headers, this app is only calling on our domain anyway
                axios.defaults.headers.common.Authorization = `Bearer ${response.data.token}`;
                localStorage.setItem("user", JSON.stringify(response.data) );

                return response.data;
            }
            return undefined;

        } catch (e){
            console.log('error: ', (e as Error).message);
            return undefined;
        }

    }
    
    static logout(): boolean {
       
        console.log("Log out!");
        axios.defaults.headers.common.Authorization = "";
        localStorage.setItem("user", "");
        return true;
    }

  
    static getCurrentUser() : IJwtLoginResponse | undefined {
        const userStr = localStorage.getItem("user");
        if(userStr) return JSON.parse(userStr);
        return undefined;
    }
}

