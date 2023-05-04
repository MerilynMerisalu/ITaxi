import { useState, MouseEvent, useEffect } from "react";
import RegisterCustomerFormView from "./RegisterCustomerFormView";
import { IRegisterCustomerData } from "../../dto/IRegisterCustomerData";
import { IdentityService } from "../../services/identiyService";
//import { DisabilityTypesService } from "../../services/disabilityTypesService";
import { Gender } from "../../utilities/enums";
import axios from "axios";
import { IDisabilityTypeData } from "../../dto/IDisabilityType";

//const [gender, setGender] = useState()
const RegisterCustomer = () => {

const today = new Date().getDate;

//const genders : string[] = Object.keys(Gender);
const [disabilityTypes, setDisabilityTypes] = useState([])
    const [values, setInput] = useState({
        Email: "",
        FirstName: "",
        LastName: "",
        Gender: Gender.Male | Gender.Female | Gender.Custom,
        DateOfBirth: "",
        DisabilityTypes: [],
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

      
      console.log("Try get data");
      //getData();

    }, []);

    const [validationErrors, setValidationErrors] = useState([] as string[])

    
 
    
    const handleChange = (target: 
      EventTarget & HTMLInputElement | 
      EventTarget & HTMLSelectElement

      ) => {

      // debugger;
      // console.log(target.name, target.value, target.type, target)
    
      setInput({ ...values, [target.name]: target.value });
      }

    const identityService = new IdentityService();
    //const disabilityTypesService = new DisabilityTypesService();

    const onSubmit = async (event: MouseEvent) => {
      console.log("onSubmit", event);
      event.preventDefault();

      if(values.Email.length === 0 || values.FirstName.length === 0 || values.LastName.length === 0 || 
        (values.Gender === Gender.Male || values.Gender === Gender.Female || values.Gender === Gender.Custom) ||
        values.DateOfBirth >= today.toString() || values.DisabilityTypeId !== "" || values.PhoneNumber.length >0 ||
        values.Password.length === 0 || values.ConfirmPassword.length === 0 || 
        values.ConfirmPassword !== values.Password ) {
          setValidationErrors(["Bad input values!"]);
          return;
        }
        // setting the initial state
      setValidationErrors([]);

      var jwtCustomerData = await identityService.registerCustomer(values);

      if (jwtCustomerData == undefined) {
        setValidationErrors(["No jwt!"]);
      } else {
        setValidationErrors([jwtCustomerData.jwt]);
      }

    }
console.log('*** DDD', disabilityTypes)
    return (
      <RegisterCustomerFormView values={values} disabilityTypes={disabilityTypes} handleChange={handleChange} onSubmit={onSubmit} validationErrors={validationErrors} />
    );

}

export default RegisterCustomer;