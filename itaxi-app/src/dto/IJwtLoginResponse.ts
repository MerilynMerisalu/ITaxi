interface IJwtLoginResponse {
    token: string 
    refreshToken: string;
    roleNames?: string[]
    firstName: string,
    lastName: string,
    firstAndLastName?: string,
    lastAndFirstName?: string
}

export type { IJwtLoginResponse };