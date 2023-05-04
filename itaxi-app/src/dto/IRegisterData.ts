import { Gender } from "../utilities/enums";

interface IRegisterData {

    Email: string,
    FirstName: string,
    LastName: string,
    Gender: Gender,
    DateOfBirth: string,
    PhoneNumber: string,
    Password: string,
    ConfirmPassword: string,

}

export type { IRegisterData }