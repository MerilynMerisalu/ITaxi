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
import VehicleCreate from "./routes/vehicles/VehicleCreate";
import VehicleDelete from "./routes/vehicles/VehicleDelete";
import VehicleEdit from "./routes/vehicles/VehicleEdit";
import VehicleGallery from "./routes/vehicles/VehicleGallery";
import ScheduleEdit from "./routes/schedules/ScheduleEdit";
import ScheduleDetails from "./routes/schedules/ScheduleDetails";
import ScheduleDelete from "./routes/schedules/ScheduleDelete";
import ScheduleCreate from "./routes/schedules/ScheduleCreate";
import DrivesIndex from "./routes/drives/DrivesIndex";
import RideTimesIndex from "./routes/rideTimes/RideTimesIndex";
import RideTimeDetails from "./routes/rideTimes/RideTimeDetails";
import RideTimeDelete from "./routes/rideTimes/RideTimeDelete";
import BookingDetails from "./routes/booking/BookingDetails";
import BookingDecline from "./routes/booking/BookingDecline";
import Print from "./routes/drives/Print";

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
        path: "vehicle/edit/:id",
        element: <VehicleEdit />,
      },
      {
        path: "vehicle/details/:id",
        element: <VehicleDetails />,
      },
      {
        path: "vehicle/delete/:id",
        element: <VehicleDelete />,
      },
      {
        path: "vehicle/gallery/:id",
        element: <VehicleGallery />,
      },
      {
        path: "vehicles/create",
        element: <VehicleCreate />
      },
      {
        path: "bookings/",
        element: <BookingIndex />
      },
      {
        path: "booking/details/:id",
        element: <BookingDetails />,
      },
      {
        path: "booking/decline/:id",
        element: <BookingDecline/>,
      },
      {
        path: "schedules",
        element: <SchedulesIndex />
      },
      {
        path: "schedules/edit/:id",
        element: <ScheduleEdit />
      },
      {
        path: "schedules/details/:id",
        element: <ScheduleDetails />
      },
      {
        path: "schedules/delete/:id",
        element: <ScheduleDelete />,
      },
      {
        path: "schedules/create",
        element: <ScheduleCreate />
      },
      {
        path: "drives",
        element: <DrivesIndex />
      },
      {
        path: "drives/print",
        element: <Print/>
      },
      {
        path: "rideTimes",
        element: <RideTimesIndex />
      },
      {
        path: "rideTimes/details/:id",
        element: <RideTimeDetails />
      },
      {
        path: "rideTimes/delete/:id",
        element: <RideTimeDelete />,
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
