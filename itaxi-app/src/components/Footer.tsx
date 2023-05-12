import { Link } from "react-router-dom";

const Footer = () => {
  return (
    <footer className="border-top footer text-muted">
      <div className="container">
        &copy; 2023 - WebApp - <Link to="/Privacy">Privacy</Link> | UI Culture:
        en-GB (en-GB) | Culture: en-GB (en-GB)
      </div>
    </footer>
  );
};

export default Footer;
