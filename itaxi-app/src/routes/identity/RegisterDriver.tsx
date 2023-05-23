import { useContext, useState } from "react";
import { IRegisterDriverData } from "../../dto/IRegisterDriverData";

import React from 'react'
import RegisterDriverFormView from "./RegisterDriverFormView";
import { IdentityService } from "../../services/IdentityService";
import { Gender } from "../../utilities/enums";
import { useNavigate } from "react-router-dom";
import { JwtContext } from "../Root";

const identityService = new IdentityService();

const RegisterDriver: React.FC = () => {
  const [cities, setCities] = useState([])
  const [driverLicenseCategories, setDriverLicenseCategory] = useState([])
  const [validationErrors, setValidationErrors] = useState([] as string[])
  const {jwtLoginResponse, setJwtLoginResponse} = useContext(JwtContext);
  const navigate = useNavigate();
  const [values, setInput] = useState({
    Email: "",
    firstName: "",
    lastName: "",
    Gender: 0,
    DateOfBirth: "",
    PersonalIdentifier: "",
    CityId: "",
    DriverLicenseNumber: "",
    DriverLicenseExpiryDate: "",
    Address: "",
    PhoneNumber: "",
    DriverLicenseCategories: [],
    Password: "",
    ConfirmPassword: "",

  } as IRegisterDriverData);

  const handleChange = (target:
    EventTarget & HTMLInputElement |
    // EventTarget & HTMLOptionsCollection |
    EventTarget & HTMLSelectElement) => {

    console.log('** TARGET', target, target.name, target.value)

    // debugger;
    // console.log(target.name, target.value, target.type, target)
      if (target.name === 'DriverLicenseCategories') {
        console.log('categories')
        const t = target as EventTarget & HTMLSelectElement
        const DriverLicenseCategories = Object.values(t.selectedOptions).map(option => option.value)
        setInput({ ...values, DriverLicenseCategories })
        return
      }
      console.log('not categories')
    setInput({ ...values, [target.name]: target.value });

  }

  const onSubmit = async (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    console.log("onSubmit", event);
    event.preventDefault();
    const driverLicenseExpiryaDateMilli = new Date(values.DriverLicenseExpiryDate)
    const dateOfBirth = Date.parse(values.DateOfBirth)
    const dateOfBirthMili = new Date(values.DateOfBirth)
    const currentDate = new Date()
    const isDateOfBirthValid = dateOfBirthMili.getTime() <= currentDate.getTime()
    const isDriverLicenseExpiryDateValid = driverLicenseExpiryaDateMilli.getTime() >= currentDate.getTime()
    console.log('*** COMPARE', isDateOfBirthValid, currentDate.getTime(), dateOfBirthMili.getTime())
    // console.log("*** SSSSS", values.Gender  , typeof values.Gender, Gender[Gender.Male],typeof Gender.Male, Gender.Male)
    console.log('*** VALUE', values, +values.Gender === Gender.Male)
    console.log(
      "*** FIND ERROR",
      values.Email.length === 0, values.firstName.length === 0, values.lastName.length === 0,
      +values.Gender === undefined,
      values.PhoneNumber.length <= 0,
      values.Password.length === 0, values.ConfirmPassword.length === 0,
      values.ConfirmPassword !== values.Password, !isDateOfBirthValid

      //console.log(values.Gender)
    )
    if (values.Email.length === 0 || values.firstName.length === 0 || values.lastName.length === 0 ||
      +values.Gender === undefined ||
      values.PhoneNumber.length <= 0 ||
      values.Password.length === 0 || values.ConfirmPassword.length === 0 ||
      values.ConfirmPassword !== values.Password || !isDateOfBirthValid || 
      values.CityId === '' || values.Address === '' || values.DriverLicenseNumber === '' 
      ||!isDriverLicenseExpiryDateValid) {
      setValidationErrors(["Bad input values!"]);
      return;
    }
    // setting the initial state
    setValidationErrors([]);

    var jwtDriverData = await identityService.registerDriver(values);

    if (jwtDriverData == undefined) {
      setValidationErrors(["No jwt!"]);
    } else {
      setValidationErrors([jwtDriverData.token]);
      setJwtLoginResponse?.(jwtDriverData)
      navigate("/")
      
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
