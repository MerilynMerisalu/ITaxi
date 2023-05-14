import "jquery";
import "popper.js";
import "bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import "font-awesome/css/font-awesome.min.css";

import "./site.css";

import React from "react";
import ReactDOM from "react-dom/client";
import { createBrowserRouter, RouterProvider, useParams } from "react-router-dom";

import Root from "./routes/Root";
import ErrorPage from "./routes/ErrorPage";
import Login from "./routes/identity/Login";
import RegisterCustomer from "./routes/identity/RegisterCustomer";
import RegisterDriver from "./routes/identity/RegisterDriver";
import Privacy from "./routes/Privacy";
import Home from "./routes/Home";
import Index from "./routes/vehicles/VehiclesIndex";
import BookingIndex from "./routes/booking/BookingIndex";
import SchedulesIndex from "./routes/schedules/SchedulesIndex";
import VehicleDetails from "./routes/vehicles/VehicleDetails";
import VehiclesIndex from "./routes/vehicles/VehiclesIndex";
import { data } from "jquery";




const router = createBrowserRouter([
  {
    path: "/",
    element: <Root />,
    errorElement: <ErrorPage />,
    children: [
      {
        path: "/",
        element: <Home />,
      },
      {
        path: "login/",
        element: <Login />,
      },
      {
        path: "registerCustomer/",
        element: <RegisterCustomer />,
      },
      {
        path: "registerDriver/",
        element: <RegisterDriver />,
      },
      {
        path: "privacy/",
        element: <Privacy />,
      },
      {
        path: "vehicles/",
        element: <VehiclesIndex />,
        
      },
      {
        path: "vehicles/:id",
        element: <VehicleDetails  />,
        
      },
      
      {
        path: "booking/index",
        element: <BookingIndex />
      },

      {
        path: "schedules/index",
        element: <SchedulesIndex />
      },

      

    ],
  },
]);

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);
root.render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
