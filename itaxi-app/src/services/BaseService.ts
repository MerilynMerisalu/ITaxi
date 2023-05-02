import Axios, { AxiosInstance } from 'axios';

 abstract class BaseService {
    private static hostBaseURL = "https://localhost:7026/api/";

    protected axios: AxiosInstance; 

    constructor (baseUrl: string){
        this.axios = Axios.create(
            {
                baseURL: BaseService.hostBaseURL + baseUrl,
                headers: {
                common: {
                    'Content-Type': 'application/json'
                    }
                }
            }
        )

    }


}

export default BaseService