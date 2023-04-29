import { useState } from "react";
import RegisterCustomerFormView from "./RegisterCustomerFormView";


const RegisterCustomer = () => {

//  const Form = () => {
    const [values, setInput] = useState({
        Email: "",
        FirstName: "",
        LastName: "",
        Gender: "",
        DateOfBirth: "",
        DisabilityTypeId: "",
        PhoneNumber: "",
        Password: "",
        ConfirmPassword: "",

    });

    const handleChange = (target: 
      EventTarget & HTMLInputElement | 
      EventTarget & HTMLSelectElement | 
      EventTarget & HTMLTextAreaElement) => {

      //debugger;
      console.log(target.name, target.value, target.type, target)
    
      setInput({ ...values, [target.name]: target.value });
      }

    return (
      <RegisterCustomerFormView values={values} handleChange={handleChange}/>
    );

}

export default RegisterCustomer;