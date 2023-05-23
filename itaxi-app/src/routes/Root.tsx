import { createContext, useState,useContext, Dispatch, SetStateAction } from "react";
import { Outlet } from "react-router-dom";
import Footer from "../components/Footer";
import Header from "../components/Header";
import { IJwtLoginResponse } from "../dto/IJwtLoginResponse";
import { data } from "jquery";
import { IdentityService } from "../services/IdentityService";

export const JwtContext = createContext
<{
  jwtLoginResponse: IJwtLoginResponse | null, 
  setJwtLoginResponse: ((data: IJwtLoginResponse | null) => void) | null,
  language: string,
  setLanguage: Dispatch<SetStateAction<string>> | null
}>
  ({jwtLoginResponse: null, setJwtLoginResponse: null, language: 'en-GB', setLanguage: null });

const Root = () => {
  const localUser = IdentityService.getCurrentUser()
  const [jwtLoginResponse, setJwtLoginResponse] = useState(localUser as IJwtLoginResponse | null );
  const [language, setLanguage] = useState<string>('en-GB')
  console.log('provider jwtloginresponse', jwtLoginResponse)
  return (
      <JwtContext.Provider value={{jwtLoginResponse, setJwtLoginResponse, language, setLanguage }}>
      <Header />
        <div className="container">
        <main role="main" className="pb-3">
          <Outlet />
        </main>
        </div>
        <Footer />
        </JwtContext.Provider> 
  );
}

/* const useJwtContext = () => useContext(JwtContext);

export { Root as default, useJwtContext}; */

export default Root
