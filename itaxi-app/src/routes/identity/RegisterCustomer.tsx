import { useState, MouseEvent, useEffect } from "react";
import RegisterCustomerFormView from "./RegisterCustomerFormView";
import { IRegisterCustomerData } from "../../dto/IRegisterCustomerData";
import { IdentityService } from "../../services/IdentityService";
//import { DisabilityTypesService } from "../../services/disabilityTypesService";
import { Gender } from "../../utilities/enums";
import axios from "axios";
import { IDisabilityTypeData } from "../../dto/IDisabilityType";
import { isDate } from "util/types";



const RegisterCustomer = () => {



const [disabilityTypes, setDisabilityTypes] = useState([])
    const [values, setInput] = useState({
        Email: "",
        FirstName: "",
        LastName: "",
        Gender: 0,
        DateOfBirth: "",
        DisabilityTypeId: "",
        PhoneNumber: "",
        Password: "",
        ConfirmPassword: "",

    } as IRegisterCustomerData);

    
    /* const getData = async () => {
      var disabilityTypes = await disabilityTypesService.getDisabilityTypes();
     
      console.log(disabilityTypes);
      return disabilityTypes;
    } */

    useEffect(()  => {
      
      // On initial load, set the select list data values
      // 1. Set the Genders lookup

      // 2. Get the Disability Types from the API (NOTE: disability types allows anonymous)
      axios.get("https://localhost:7026/api/v1/adminarea/disabilityTypes", {
        headers: {
          "Content-Type": "application/json"
        }
      }).then(response => {
        console.log('*** RES', response)
         setDisabilityTypes(response.data);
      });

      const today = new Date().getDate()
console.log(today)
      console.log("Try get data");
      //getData();

    }, []);

    const [validationErrors, setValidationErrors] = useState([] as string[])

    
 
    
    const handleChange = (target: 
      EventTarget & HTMLInputElement | 
      EventTarget & HTMLSelectElement

      ) => {

        console.log('** TARGET', target, target.value)

      // debugger;
      // console.log(target.name, target.value, target.type, target)
    
      setInput({ ...values, [target.name]: target.value });
      
      }

    const identityService = new IdentityService();
    //const disabilityTypesService = new DisabilityTypesService();

    const onSubmit = async (event: MouseEvent) => {
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
         values.DisabilityTypeId === "" , 
         values.PhoneNumber.length <= 0 ,
      values.Password.length === 0 , values.ConfirmPassword.length === 0 ,
       values.ConfirmPassword !== values.Password, !isDateOfBirthValid
 
    //console.log(values.Gender)
)
      if(values.Email.length === 0 || values.FirstName.length === 0 || values.LastName.length === 0 || 
        +values.Gender === Gender.undefined ||
         values.DisabilityTypeId === "" || values.PhoneNumber.length <= 0 ||
        values.Password.length === 0 || values.ConfirmPassword.length === 0 || 
        values.ConfirmPassword !== values.Password || !isDateOfBirthValid   ) {
          setValidationErrors(["Bad input values!"]);
          return;
        } 
        // setting the initial state
      setValidationErrors([]);

      var jwtCustomerData = await identityService.registerCustomer(values);

      if (jwtCustomerData == undefined) {
        setValidationErrors(["No jwt!"]);
      } else {
        setValidationErrors([jwtCustomerData.token]);
      }

    }
console.log('*** DDD', disabilityTypes)
    return (
      <RegisterCustomerFormView values={values} disabilityTypes={disabilityTypes} handleChange={handleChange} onSubmit={onSubmit} validationErrors={validationErrors} />
    );

}

export default RegisterCustomer;