interface IJwtLoginResponse{
    token: string 
    refreshToken: string;
    roleNames: string[]
    firstName: string,
    lastName: string
 
}

export type { IJwtLoginResponse };