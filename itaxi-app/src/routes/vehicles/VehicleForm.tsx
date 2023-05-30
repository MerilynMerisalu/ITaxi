import {
  ChangeEvent,
  useState,
  useEffect,
  useContext,
  FormEvent,
} from "react";
import { VehicleService } from "../../services/VehicleService";
import { VehicleTypeService } from "../../services/VehicleTypeService";
import { IVehicleType } from "../../domain/IVehicleType";
import { IVehicleMark } from "../../domain/IVehicleMark";
import { IVehicleFormData } from "../../dto/IVehicleFormData";
import { JwtContext } from "../Root";
import axios from "axios";
import { VehicleMarkService } from "../../services/VehicleMarkService";
import { VehicleModelService } from "../../services/VehicleModelService";
import { IVehicleModel } from "../../domain/IVehicleModel";
import { VehicleAvailability } from "../../utilities/enums";

const availabilityEntries = Object.entries(VehicleAvailability);
const service = new VehicleService();
const vehicleTypeService = new VehicleTypeService();
const vehicleMarkService = new VehicleMarkService();
const vehicleModelService = new VehicleModelService();
const VehicleForm = ({
  action,
  initialValues,
}: {
  action: (values: IVehicleFormData) => Promise<void>;
  initialValues: IVehicleFormData
}) => {
  const [values, setValues] = useState(initialValues);
  useEffect(() => {
    console.log('values effect', initialValues)
    setValues(initialValues)
  }, [initialValues])

  const { language } = useContext(JwtContext);
  const [vehicleTypes, setVehicleTypes] = useState<IVehicleType[]>();
  const [manufactureYears, setManufactureYears] = useState<number[]>();
  const [marks, setMarks] = useState<IVehicleMark[]>();
  const [models, setModels] = useState<IVehicleModel[]>();

  useEffect(() => {
    async function downloadYears() {
      const years = await service.getManufactureYears();
      setManufactureYears(years);
    }
    async function downloadVehicleTypes() {
      const vehicleTypes = await vehicleTypeService.getAll();
      setVehicleTypes(vehicleTypes);
    }
    async function downloadMarks() {
      const marks = await vehicleMarkService.getAll();
      setMarks(marks);
    }
    async function downloadModels() {
      const models = await vehicleModelService.getAll();
      setModels(models);
    }
    async function download() {
      const promises = [
        downloadYears(),
        downloadVehicleTypes(),
        downloadMarks(),
        downloadModels(),
      ];
      await Promise.all(promises);
      console.log("download complete");
    }
    download();
    axios.defaults.headers.common["Accept-Language"] = language;
  }, [language]);
  function handleChange(
    event: ChangeEvent<HTMLSelectElement | HTMLInputElement>
  ) {
    setValues((currentValues) => {
        const value = event.target.name === 'vehicleAvailability' ? Number(event.target.value): event.target.value
      return {
        ...currentValues,
        [event.target.name]: value,
      };
    });
  }
  const vehicleTypeViews = vehicleTypes?.map((option) => (
    <option key={option.id} value={option.id}>
      {option.vehicleTypeName}
    </option>
  ));
  const markViews = marks?.map((option) => (
    <option key={option.id} value={option.id}>
      {option.vehicleMarkName}
    </option>
  ));
  const filteredModelOptions =
    values.vehicleMarkId === ""
      ? models
      : models?.filter(
          (option) => option.vehicleMarkId === values.vehicleMarkId
        );
  const modelViews = filteredModelOptions?.map((option) => (
    <option key={option.id} value={option.id}>
      {option.vehicleModelName}
    </option>
  ));
  const yearOptionViews = manufactureYears?.map((year) => (
    <option key={year} value={year}>
      {year}
    </option>
  ));
  const availabilityOptionViews = availabilityEntries.map((entry, index) => {
    const value = index + 1;
    return (
      <option key={value} value={value}>
        {entry[1]}
      </option>
    );
  });
  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault();
    await action(values);
  };
  return (
    <form onSubmit={handleSubmit}>
      <div className="text-danger validation-summary-valid">
        <ul>
          <li style={{ display: "none" }}></li>
        </ul>
      </div>

      <div className="form-group">
        <label className="control-label" html-for="VehicleTypeId">
          Vehicle Type
        </label>
        <select
          className="form-control"
          id="VehicleTypeId"
          name="vehicleTypeId"
          value={values.vehicleTypeId}
          onChange={handleChange}
        >
          <option>Please Select</option>
          {vehicleTypeViews}
        </select>
      </div>
      <div className="form-group">
        <label className="control-label" html-for="VehicleMarkId">
          Vehicle Mark
        </label>
        <select
          className="form-control"
          id="VehicleMarkId"
          name="vehicleMarkId"
          value={values.vehicleMarkId}
          onChange={handleChange}
        >
          <option>Please Select</option>
          {markViews}
        </select>
      </div>
      <div className="form-group">
        <label className="control-label" html-for="VehicleModelId">
          Vehicle Model
        </label>
        <select
          className="form-control"
          id="VehicleModelId"
          name="vehicleModelId"
          value={values.vehicleModelId}
          onChange={handleChange}
        >
          <option>Please Select</option>
          {modelViews}
        </select>
      </div>
      <div className="form-group">
        <label className="control-label" html-for="VehiclePlateNumber">
          Vehicle Plate Number
        </label>
        <input
          className="form-control"
          type="text"
          id="VehiclePlateNumber"
          maxLength={25}
          name="vehiclePlateNumber"
          value={values.vehiclePlateNumber}
          onChange={handleChange}
        />
        <span className="text-danger field-validation-valid"></span>
      </div>
      <div className="form-group">
        <label className="control-label" html-for="ManufactureYear">
          Year
        </label>
        <select
          className="form-control"
          id="ManufactureYear"
          name="manufactureYear"
          value={values.manufactureYear}
          onChange={handleChange}
        >
          <option>Please Select</option>
          {yearOptionViews}
        </select>
        <span className="text-danger field-validation-valid"></span>
      </div>
      <div className="form-group">
        <label className="control-label" html-for="NumberOfSeats">
          Number of Seats
        </label>
        <input
          className="form-control"
          min="2"
          max="6"
          type="number"
          id="NumberOfSeats"
          name="numberOfSeats"
          value={values.numberOfSeats}
          onChange={handleChange}
        />
        <input name="__Invariant" type="hidden" value="NumberOfSeats" />
        <span className="text-danger field-validation-valid"></span>
      </div>
      <div className="form-group">
        <label className="control-label" html-for="VehicleAvailability">
          Vehicle Availability
        </label>
        <select
          className="form-control"
          id="VehicleAvailability"
          name="vehicleAvailability"
          value={values.vehicleAvailability}
          onChange={handleChange}
        >
          <option>Please Select</option>
          {availabilityOptionViews}
        </select>
        <span className="text-danger field-validation-valid"></span>
      </div>
      <div className="form-group">
        <input type="submit" value="Create" className="btn btn-primary" />
      </div>
      <input
        name="__RequestVerificationToken"
        type="hidden"
        value="CfDJ8H6gnGQdd_VPhYRnzYmPi0pQAlFx34AR8fQjXmuL4pIHZx8D6fVO86XxC59WOSnIfAqKM8NRzvVGTEd6Qt8wRzTl9i9_h0fkoEwBt0qfGIkzgv969qIR9_q2KHyTjPH4PCyDBsudUKZie3JyyWrQFNxtWwTTmmGEF7C0nwvkpwKZXbfpqFKmrINqV2oarmOAkg"
      />
    </form>
  );
};

export default VehicleForm;
