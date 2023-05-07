import { useState, useContext, MouseEvent} from "react";
import { ILoginData } from "../../dto/ILoginData";
import { IdentityService } from "../../services/IdentiyService";
import { JwtContext } from "../Root";
import LoginFormView from "../LoginFormView";
import { useNavigate } from "react-router-dom";
import { isJSDocUnknownTag } from "typescript";


const Login = () => {
   const navigate = useNavigate();

   const [values, setInput] = useState({
        Email: "",
        Password: "",
    } as ILoginData);

    const [validationErrors, setValidationErrors] = useState([] as string[]); 
   
    const handleChange = (target: EventTarget & HTMLInputElement) => {

      // debugger;
      // console.log(target.name, target.value, target.type, target)
    
      setInput({ ...values, [target.name]: target.value });
    }

    const {jwtLoginResponse, setJwtLoginResponse} =useContext(JwtContext);
    
    const identityService = new IdentityService();
    

    const onSubmit = async (event: MouseEvent) => {
      console.log('onSubmit', event);
      event.preventDefault();
      
      if(values.Email.length == 0 || values.Password.length == 0  ) {
          setValidationErrors(["Bad input values!"]);
          return;
      }

      // remove errors
      setValidationErrors([]);

      var jwtLoginData = await identityService.login(values);

      if (jwtLoginData == undefined) {
        setValidationErrors(["No jwt!"]);
        return;
      } 

      if (setJwtLoginResponse){
        setJwtLoginResponse(jwtLoginData);
        navigate("/");
      }
    }
    
    return(
      <LoginFormView values={values} handleChange={handleChange} onSubmit={onSubmit} validationErrors={validationErrors} />
    );   

}

export default Login;