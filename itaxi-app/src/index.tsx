import 'jquery';
import 'popper.js';
import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'; 
import 'font-awesome/css/font-awesome.min.css';
import './site.css';
import React from 'react';
import ReactDOM from 'react-dom/client';
import Root from './routes/Root';


const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <React.StrictMode>
      <Root />
  </React.StrictMode>
);


