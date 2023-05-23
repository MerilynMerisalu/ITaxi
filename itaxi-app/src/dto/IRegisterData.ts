import { Gender } from "../utilities/enums";


interface IRegisterData {

    Email: string,
    firstName: string,
    lastName: string,
    firstAndLastName?: string,
    lastAndFirstName?: string
    Gender: number,
    DateOfBirth: string,
    PhoneNumber: string,
    Password: string,
    ConfirmPassword: string,
    roleNames?: string[]
}

export type { IRegisterData }