import { useState } from "react";
import { IRegisterDriverData } from "../../dto/IRegisterDriverData";

import React from 'react'
import RegisterDriverFormView from "./RegisterDriverFormView";
import { IdentityService } from "../../services/IdentityService";
import { Gender } from "../../utilities/enums";

const identityService = new IdentityService();

const RegisterDriver: React.FC = ( ) => {
  const [cities, setCities] = useState([])
  const [driverLicenseCategories, setDriverLicenseCategory] = useState([])
  const [validationErrors, setValidationErrors] = useState([] as string[])

    const [values, setInput] = useState({
        Email: "",
        FirstName: "",
        LastName: "",
        Gender: 0,
        DateOfBirth: "",
        PersonalIdentifier: "",
        CityId: "",
        DriverLicenseNumber: "",
        DriverLicenseExpiryDate: "",
        Address: "",
        PhoneNumber: "",
        DriverLicenseCategoryIds: [],
        DriverLicenseCategoryId: "",
        Password: "",
        ConfirmPassword: "",

    } as IRegisterDriverData);

    const handleChange = (target: 
      EventTarget & HTMLInputElement | 
      // EventTarget & HTMLOptionsCollection |
      EventTarget & HTMLSelectElement) => {

        console.log('** TARGET', target, target.value)

      // debugger;
      // console.log(target.name, target.value, target.type, target)
    
      setInput({ ...values, [target.name]: target.value });
      
      }

      const onSubmit = async (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        console.log("onSubmit", event);
        event.preventDefault();
        const dateOfBirth = Date.parse(values.DateOfBirth)
        const dateOfBirthMili = new Date(values.DateOfBirth)
        const currentDate = new Date()
        const isDateOfBirthValid = dateOfBirthMili.getTime() <= currentDate.getTime()
      console.log('*** COMPARE', isDateOfBirthValid , currentDate.getTime(), dateOfBirthMili.getTime() )
     // console.log("*** SSSSS", values.Gender  , typeof values.Gender, Gender[Gender.Male],typeof Gender.Male, Gender.Male)
     console.log('*** VALUE', values, +values.Gender === Gender.Male)
  console.log(
  "*** FIND ERROR",
    values.Email.length === 0 , values.FirstName.length === 0 , values.LastName.length === 0 ,
          +values.Gender === Gender.undefined ,
           values.PhoneNumber.length <= 0 ,
        values.Password.length === 0 , values.ConfirmPassword.length === 0 ,
         values.ConfirmPassword !== values.Password, !isDateOfBirthValid
   
      //console.log(values.Gender)
  )
        if(values.Email.length === 0 || values.FirstName.length === 0 || values.LastName.length === 0 || 
          +values.Gender === Gender.undefined ||
          values.PhoneNumber.length <= 0 ||
          values.Password.length === 0 || values.ConfirmPassword.length === 0 || 
          values.ConfirmPassword !== values.Password || !isDateOfBirthValid   ) {
            setValidationErrors(["Bad input values!"]);
            return;
          } 
          // setting the initial state
        setValidationErrors([]);
  
        var jwtCustomerData = await identityService.registerDriver(values);
  
        if (jwtCustomerData == undefined) {
          setValidationErrors(["No jwt!"]);
        } else {
          setValidationErrors([jwtCustomerData.token]);
        }
  
      }

    return <RegisterDriverFormView 
    values={values} 
    cities={cities} 
    driverLicenseCategories={driverLicenseCategories}
    onSubmit={onSubmit}
    handleChange={handleChange}
    validationErrors={validationErrors} 
    />
  }
    

export default RegisterDriver
