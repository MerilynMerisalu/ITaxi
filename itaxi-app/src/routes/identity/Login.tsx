import { useState, MouseEvent} from "react";
import { ILoginData } from "../../dto/ILoginData";
import { IdentityService } from "../../services/identiyService";
import LoginFormView from "../LoginFormView";

const Login = () => {

   const [values, setInput] = useState({
        Email: "",
        Password: "",
        

    } as ILoginData);

    const [validationErrors, setValidationErrors] = useState([] as string[])

    
 
    
    const handleChange = (target: 
      EventTarget & HTMLInputElement

      ) => {

      // debugger;
      // console.log(target.name, target.value, target.type, target)
    
      setInput({ ...values, [target.name]: target.value });
      
      }

    const identityService = new IdentityService();
    

    const onSubmit = async (event: MouseEvent) => {
      console.log("onSubmit", event);
      event.preventDefault();
      
      if(values.Email.length === 0 || values.Password.length === 0  ) {
          setValidationErrors(["Bad input values!"]);
          return;
        }
        // setting the initial state
      setValidationErrors([]);

      var jwtLoginData = await identityService.login(values);

      if (jwtLoginData == undefined) {
        setValidationErrors(["No jwt!"]);
      } else {
        setValidationErrors([jwtLoginData.jwt]);
      }

    }
    return (
      <LoginFormView values={values} handleChange={handleChange} onSubmit={onSubmit} validationErrors={validationErrors} />
    )
}

export default Login;