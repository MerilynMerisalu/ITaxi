// import { DateTimePicker } from "@mui/x-date-pickers";
import {
    ChangeEvent,
    useState,
    useEffect,
    useContext,
    FormEvent,
  } from "react";
  

import { IScheduleFormData } from "../../dto/IScheduleFormData";
import { JwtContext } from "../Root";
import { IVehicle } from "../../domain/IVehicle";
import axios from "axios";
import { ScheduleService } from "../../services/ScheduleService";
import { VehicleService } from "../../services/VehicleService";
import BasicDateTimePicker from "../../components/DateTimePicker";

const service = new ScheduleService();
const vehicleService = new VehicleService();
interface IProps {
  values: IScheduleFormData;
}
  const ScheduleForm = ({
    action,
    initialValues,
    id,
  }: {
    action: (values: IScheduleFormData ) => Promise<void>;
    initialValues: IScheduleFormData | null,
    id?: string
  }) => {
    const [startDateAndTime, setStartDateAndTime] = useState('')
    const [endDateAndTime, setEndDateAndTime] = useState('')
    const [values, setValues] = useState(initialValues);
    const [vehicles, setVehicles] = useState<IVehicle[]>();

    useEffect(() => {
      console.log('values effect', initialValues)

      setValues(initialValues)
    }, [initialValues])


  
    const { language } = useContext(JwtContext);
    console.log('*** VEH', vehicles)
  
    useEffect(() => {
      async function downloadVehicles() {
        const vehicles = await vehicleService.getAll();
        setVehicles(vehicles);
      }
      
      async function download() {
        const promises = [
          downloadVehicles()
        ];
        await Promise.all(promises);
        console.log("download complete");
      }
      download();
      axios.defaults.headers.common["Accept-Language"] = language;
    }, [language]);
    function handleChange(
      event: ChangeEvent<EventTarget & HTMLInputElement | 
      EventTarget & HTMLSelectElement >
    ) {
      let value: string
      console.log('*** CHANGE', values, event, event.target.value)
      setValues((currentValues) => {
          
        return {
          ...currentValues,
          [event.target.name]: event.target.value,
        };
      });
    }
    const vehiclesViews = vehicles?.map((option) => (
      <option key={option.id} value={option.id} >
        {option.vehicleIdentifier}
      </option>
    ));
    
    
    const handleSubmit = async (event: FormEvent) => {
      event.preventDefault();
      console.log('*** REQ', values, startDateAndTime, endDateAndTime)
      const body = {
        vehicleId: values?.vehicleId,
        startDateAndTime,
        endDateAndTime
      }
      await action(body);
    };
const datePickerHandler =(date: string, type: string) => {
  console.log('*** date picker data',date);
  if(type === 'start'){

    setStartDateAndTime(date)
  } else {
    setEndDateAndTime(date)
  }


}

console.log('*** DDDD', startDateAndTime, endDateAndTime)
return(
    <form onSubmit={handleSubmit}>
                            <div className="text-danger validation-summary-valid"><ul><li style={{ display: "none" }}></li>
                            </ul></div>
                            <div className="form-group">
                                <label className="control-label" html-for="VehicleId">Vehicle</label>
                                <select  className="form-control"
                                    id="vehicleId"
                                    name="vehicleId"
                                    value={values?.vehicleId}
                                    onChange={(value) =>  handleChange(value)}>
                                    <option>Please Select</option>
                                    {vehiclesViews}
                                </select>
                                
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="StartDateAndTime">Shift Start Date and Time</label>
                                
                                 <BasicDateTimePicker type="start" onChange={datePickerHandler} /> 
                                  {/* <input className="form-control" type="datetime-local"
                                  id="startDateAndTime" name="startDateAndTime" 
                                  value={values.startDateAndTime}  onChange={handleChange} />
                                  <input name="__Invariant" type="hidden" value="StartDateAndTime" /> 
                                <span className="text-danger field-validation-valid"></span> */}
                            </div> 
                            
                             <div className="form-group">
                                <label className="control-label" html-for="EndDateAndTime">Shift End Date and Time</label>
                                 {/*<input className="form-control" type="datetime-local" id="endDateAndTime" name="endDateAndTime" 
                                 value={values.endDateAndTime} onChange={handleChange}  />
                                 <input name="__Invariant" type="hidden" value="EndDateAndTime" />  */}
                                <BasicDateTimePicker type="end" onChange={datePickerHandler} />

                                <span className="text-danger field-validation-valid"></span>
                            </div>

                            <div className="form-group">
                                <input type="submit" value="Create" className="btn btn-primary" />
                            </div>
                            <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0qOkkzEhj0KnosSlxhdMwvLbphwX0YS2r0A0tbjOuKPtfmQV0HMqLyTEcN4vAq4IRlo6K_QJeA1eib-H89O8ynWgjpuzmxV3b-JsKEGHyHRNZz3nR1BYLCtaeDKazfzKQdvj55QAiaTw3IjAdRmE1AYZnR4_99PK8QT37Gb7cte0A" />
                        </form>
)

                        
  }
  export default ScheduleForm