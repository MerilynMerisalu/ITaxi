import React, {
  ChangeEvent,
  useState,
  useEffect,
  useContext,
  FormEvent,
} from "react";
import { Link } from "react-router-dom";
import { VehicleService } from "../../services/VehicleService";
import { VehicleTypeService } from "../../services/VehicleTypeService";
import { useNavigate } from "react-router-dom";

import { IVehicleType } from "../../domain/IVehicleType";
import { IVehicleMark } from "../../domain/IVehicleMark";
import { IVehicleData } from "../../dto/IVehicleData";
import { IVehicleFormData } from "../../dto/IVehicleFormData";
import { JwtContext } from "../Root";

import axios from "axios";
import { VehicleMarkService } from "../../services/VehicleMarkService";
import { VehicleModelService } from "../../services/VehicleModelService";
import { IVehicleModel } from "../../domain/IVehicleModel";

import { VehicleAvailability } from "../../utilities/enums";
import VehicleForm from "./VehicleForm";
const availabilityEntries = Object.entries(VehicleAvailability);

const service = new VehicleService();

const VehicleCreate = () => {
  const navigate = useNavigate();
  const createAction = async (values: IVehicleFormData) => {
    console.log("values test:", values);
    const status = await service.create(values);
    console.log("status:", status);
    if (status === 201 || status === 200) {
      console.log("status ok");
      navigate("/vehicles");
    } else {
      console.warn("Vehicle create not OK", status);
    }
  };
  return (
    <div className="container">
      <main role="main" className="pb-3">
        <h1>Create</h1>

        <h4>Vehicle</h4>
        <hr />
        <div className="row">
          <div className="col-md-4">
            <VehicleForm
              action={createAction}
              initialValues={{
                vehicleTypeId: "",
                vehicleMarkId: "",
                vehicleModelId: "",
                manufactureYear: "",
                numberOfSeats: "2",
                vehiclePlateNumber: "",
                vehicleAvailability: 0,
              }}
            />
          </div>
        </div>

        <div>
          <Link to={"/vehicles"}>Back to List</Link>
        </div>
      </main>
    </div>
  );
};

export default VehicleCreate;
