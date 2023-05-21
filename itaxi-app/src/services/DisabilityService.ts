import { IDisabilityTypeData } from "../dto/IDisabilityTypeData";

import BaseService from "./BaseService";

 export class DisabilityTypesService extends BaseService {
    constructor(){
        super('v1/adminArea/');
    }
    
    async getDisabilityTypes(): Promise<IDisabilityTypeData[] | undefined> {
        try {
            const response = await this.axios.get<IDisabilityTypeData[]>('disabilityTypes');

            console.log('disabilityTypes response', response);
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
