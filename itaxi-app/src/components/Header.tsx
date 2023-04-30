import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';

function BasicExample() {
  return (
    <Navbar className='navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3'>
      <Container fluid>
      <Navbar.Brand href="/">ITaxi</Navbar.Brand>
        <Navbar.Toggle aria-controls="navbar-nav me-auto mb-2 mb-lg-0" />
        <Navbar.Collapse id="navbarSupportedContent" >
          <Nav className="me-auto">
            <Nav.Link className="nav-link active" href="/">Home</Nav.Link>
            <Nav.Link href="privacy" className="nav-link active">Privacy</Nav.Link>
            <NavDropdown title="Languages" id="basic-nav-dropdown">
              <NavDropdown.Item href="/Home/SetLanguage?culture=en-GB&amp;returnUrl=%2F">English (United Kingdom)</NavDropdown.Item>
              <NavDropdown.Item href="/Home/SetLanguage?culture=et-EE&amp;returnUrl=%2F">
              eesti (Eesti)
              </NavDropdown.Item>
              
            </NavDropdown>
          </Nav>
          <Nav>
            <Nav.Link className="nav-link text-dark" href="registerAdmin">Register Admin</Nav.Link>
            <Nav.Link className="nav-link text-dark" href="registerDriver">Register Driver</Nav.Link>
            <Nav.Link className="nav-link text-dark" href="registerCustomer" >Register Customer</Nav.Link>
            <Nav.Link className="nav-link text-dark" href="login">Login</Nav.Link>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}

export default BasicExample;