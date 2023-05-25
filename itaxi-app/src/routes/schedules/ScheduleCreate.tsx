import React from 'react'
import { Link, useNavigate } from 'react-router-dom'
import DateTimePicker from '../../components/DateTimePicker'
import ScheduleForm from './ScheduleForm'
import { IScheduleFormData } from '../../dto/IScheduleFormData'
import { ScheduleService } from '../../services/ScheduleService'


const service = new ScheduleService()

const ScheduleCreate = () => {
    const navigate = useNavigate()
    const createAction = async (values: IScheduleFormData) => {
        console.log("values test:", values);
        const status = await service.create(values);
        console.log("status:", status);
        if (status === 201 || status === 200) {
          console.log("status ok");
          navigate("/schedules");
        } else {
          console.warn("Vehicle create not OK", status);
        }
      };
    return (
        <div className="container">
            <main role="main" className="pb-3">
                <h1>Create</h1>

                <h4>Schedule</h4>
                <hr />
                
                <div className="row">
                    <div className="col-md-4">
                    <ScheduleForm
                    action={createAction}
              initialValues={{
                vehicleId: "",
                startDateAndTime: "",
                endDateAndTime: ""
              }}
            />
                    </div>
                </div>

                <div>
                    <Link to={"/schedules"}>Back to List</Link>
                </div>
            </main>
        </div>
    )
}

export default ScheduleCreate