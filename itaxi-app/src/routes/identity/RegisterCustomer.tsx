import { useState, MouseEvent } from "react";
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

    const [validationErrors, setValidationErrors] = useState([] as string[])


 
    
    const handleChange = (target: 
      EventTarget & HTMLInputElement | 
      EventTarget & HTMLSelectElement

      ) => {

      //debugger;
      console.log(target.name, target.value, target.type, target)
    
      setInput({ ...values, [target.name]: target.value });
      }

    const onSubmit = (event: MouseEvent) => {
      console.log("onSubmit", event);
      event.preventDefault();

      if(values.Email.length === 0 || values.FirstName.length === 0 || values.LastName.length === 0 ||
        values.Password.length === 0 || values.ConfirmPassword.length === 0 || values.ConfirmPassword !== values.Password ) {
          setValidationErrors(["Bad input values!"]);
          return;
        }
        // setting the initial state
      setValidationErrors([]);
    }

    return (
      <RegisterCustomerFormView values={values} handleChange={handleChange} onSubmit={onSubmit} validationErrors={validationErrors} />
    );

}

export default RegisterCustomer;