import { useContext, useEffect, useState } from 'react';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';
import { JwtContext } from '../routes/Root';
import { Link, useNavigate } from 'react-router-dom';
import { Button } from 'bootstrap';
import axios from 'axios';
import { IdentityService } from '../services/IdentityService';
import { IJwtLoginResponse } from '../dto/IJwtLoginResponse';

function Header() {
  const { setLanguage, jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
  const navigate = useNavigate();
  const [user, setUser] = useState<IJwtLoginResponse>();
  useEffect(() => {
    const newUser : IJwtLoginResponse | undefined = IdentityService.getCurrentUser();
  
    return () => {
      setUser(newUser)
    }
  }, []);

  let currentuser = jwtLoginResponse ?? IdentityService.getCurrentUser()

const languageButtonHandler = (event: React.MouseEvent<HTMLButtonElement>) => {
  //event.preventDefault();

  const button: HTMLButtonElement = event.currentTarget;
  let lang = button.name;
  setLanguage?.(lang)
  IdentityService.setLanguage(lang);
  console.log(`User selected language: '${lang}'`);
  axios.defaults.headers.common['Accept-Language'] = lang;

  // window.location.reload()
}

const logoutButtonHandler = (event: React.MouseEvent<HTMLButtonElement>) => {
  
  
  console.log(`User clicked logout.`);
  if(IdentityService.logout())
  {
    setJwtLoginResponse?.(null)
      navigate("/");
  }
}

const displayIfNotLoggedIn= () => {
  //var user = IdentityService.getCurrentUser();
  return {'display': currentuser == null ? '' : 'none'}
}

const displayIfLoggedIn= () => {
  //var user = IdentityService.getCurrentUser();
  return {'display': currentuser == null ? 'none' : ''}
};

const displayIfLoggedInRole= (role : string) => {
  //var user = IdentityService.getCurrentUser();
  return {'display': currentuser != null && currentuser.roleNames?.[0] === role ? '' : 'none'  }
};

  return (
    <Navbar className='navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3'>
      <Container fluid>
      <Navbar.Brand as={Link} to="/">ITaxi</Navbar.Brand>
        <Navbar.Toggle aria-controls="navbar-nav me-auto mb-2 mb-lg-0" />
        <Navbar.Collapse id="navbarSupportedContent" >
          <Nav className="me-auto">
            <Nav.Link as={Link} to="/" className="nav-link active">Home</Nav.Link>
            <NavDropdown title="Languages" id="basic-nav-dropdown">
              <NavDropdown.Item name="en-GB" onClick={languageButtonHandler}>English (United Kingdom)</NavDropdown.Item>
              <NavDropdown.Item name="et" onClick={languageButtonHandler}>Eesti (Eesti)</NavDropdown.Item>
            </NavDropdown>

            <NavDropdown style={displayIfLoggedInRole("Driver")} title="Activities" id="basic-nav-dropdown">
              <NavDropdown.Item as={Link} to="/vehicles">Vehicles</NavDropdown.Item>
              <NavDropdown.Item as={Link} to="/schedules/">Schedules</NavDropdown.Item>
              <NavDropdown.Item as={Link} to="/rideTimes">RideTimes</NavDropdown.Item>
              <NavDropdown.Item as={Link} to="/drives">Drives</NavDropdown.Item>
              <NavDropdown.Item as={Link} to="/photos">Photos</NavDropdown.Item>
            
            </NavDropdown>
            <NavDropdown style={displayIfLoggedInRole("Customer")} title="Activities" id="basic-nav-dropdown">
              <NavDropdown.Item as={Link} to="/bookings/">Bookings</NavDropdown.Item>
              <NavDropdown.Item as={Link} to="/comments">Comments</NavDropdown.Item>
              
            
            </NavDropdown>
          </Nav>
          <Nav style={displayIfNotLoggedIn()}>
            
            {/* <Nav.Link className="nav-link text-dark" as={Link} to="registerAdmin">Register Admin</Nav.Link> */}
            <Nav.Link className="nav-link text-dark" as={Link} to="registerDriver">Register Driver</Nav.Link>
            <Nav.Link className="nav-link text-dark" as={Link} to="registerCustomer">Register Customer</Nav.Link>
            <Nav.Link className="nav-link text-dark" as={Link} to="login">Login</Nav.Link>
          </Nav>
          <Nav style={displayIfLoggedIn()} onClick={logoutButtonHandler}>
          <Nav.Link style={displayIfLoggedIn()}  as= {Link} to="#">{`Hello, ${currentuser?.firstAndLastName}!`}</Nav.Link> 
          <Nav.Link className="nav-link text-dark" as={Link} to="/">Logout</Nav.Link>

          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}

export default Header;