import { Link } from "react-router-dom";

const Header = () => {
  return (
    <header>
      <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
        <div className="container">
          <a className="navbar-brand" href="/">
            ITaxi
          </a>
          <button
            className="navbar-toggler"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target=".navbar-collapse"
            aria-controls="navbarSupportedContent"
            aria-expanded="false"
            aria-label="Toggle navigation"
          >
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className="collapse navbar-collapse" id="navbarSupportedContent">
            <ul className="navbar-nav me-auto mb-2 mb-lg-0">
              <li className="nav-item">
                <Link to="/" className="nav-link active">Home</Link>
              </li>
              <li className="nav-item">
                <Link to="privacy" className="nav-link active">Privacy</Link>
              </li>

              <li className="nav-item dropdown">
                <a
                  className="nav-link dropdown-toggle"
                  href="/#"
                  id="navbarDropdown"
                  role="button"
                  data-bs-toggle="dropdown"
                  aria-expanded="false"
                >
                  Languages
                </a>
              </li>

              <ul className="dropdown-menu" aria-labelledby="navbarDropdown">
                <nav className="nav-item">
                  <a className="dropdown-item" href="/#">
                    <a
                      className="dropdown-item"
                      href="/Home/SetLanguage?culture=en-GB&amp;returnUrl=%2F"
                    >
                      English (United Kingdom)
                    </a>
                    <a
                      className="dropdown-item"
                      href="/Home/SetLanguage?culture=et-EE&amp;returnUrl=%2F"
                    >
                      eesti (Eesti)
                    </a>
                    <a
                      className="dropdown-item"
                      href="/Home/SetLanguage?culture=ru-RU&amp;returnUrl=%2F"
                    >
                      &#x440;&#x443;&#x441;&#x441;&#x43A;&#x438;&#x439;
                      (&#x420;&#x43E;&#x441;&#x441;&#x438;&#x44F;)
                    </a>
                    <a
                      className="dropdown-item"
                      href="/Home/SetLanguage?culture=lv-LV&amp;returnUrl=%2F"
                    >
                      latvie&#x161;u (Latvija)
                    </a>
                    <a
                      className="dropdown-item"
                      href="/Home/SetLanguage?culture=lt-LT&amp;returnUrl=%2F"
                    >
                      lietuvi&#x173; (Lietuva)
                    </a>
                  </a>
                </nav>
              </ul>
            </ul>
          </div>

          <ul className="navbar-nav">
            <li className="nav-item">
              <a
                className="nav-link text-dark"
                href="/Identity/Account/RegisterAdmin"
              >
                Register Admin
              </a>
            </li>
            <li className="nav-item">
              <Link to="registerDriver" className="nav-link text-dark">Register Driver</Link>
            </li>
            <li className="nav-item">
              <Link to="registerCustomer" className="nav-link text-dark">Register Customer</Link>
            </li>
            <li className="nav-item">
              <Link to="login" className="nav-link text-dark">Login</Link>
            </li>
          </ul>
        </div>
      </nav>
    </header>
  );
};
export default Header;
