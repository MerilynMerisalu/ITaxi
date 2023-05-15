import React, { useContext, useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom';
import { JwtContext } from '../Root';
import { ScheduleService } from '../../services/ScheduleService';
import { ISchedule } from '../../domain/ISchedule';


const ScheduleDetails = () => {
    const { id } = useParams();
    const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState<ISchedule | null>(null)
    const scheduleService = new ScheduleService();
    console.log('data test:', data)
    useEffect(() => {
        console.log('jwtloginresponse', jwtLoginResponse)
        if (jwtLoginResponse) {
            scheduleService.details(id)
                .then(
                    response => {
                        console.log(`Schedule: ${response}`)
                        if (response)
                            setData(response)
                        else {
                            setData(null)
                        }
                    }
                )
        }

    }, [id]);

    return (
        <div className="container">
            <main role="main" className="pb-3">

                <h1>Details</h1>

                <div>
                    <h4>Schedule</h4>
                    <hr />


                    <dl className="row">
                        <dt className="col-sm-2">
                            Vehicle
                        </dt>
                        <dd className="col-sm-10">
                            {data?.vehicle.vehicleIdentifier}
                        </dd>
                        <dt className="col-sm-2">
                            Shift Start Date and Time
                        </dt>
                        <dd className="col-sm-10">
                            {data?.startDateAndTime}
                        </dd>
                        <dt className="col-sm-2">
                            Shift End Date and Time
                        </dt>
                        <dd className="col-sm-10">
                            {data?.endDateAndTime}
                        </dd>

                    </dl>
                </div>
                <div>
                    <a href="/DriverArea/Schedules/Edit/d88e5375-314a-4c3d-f505-08db556c3975">Edit</a> |
                    <Link to={"/schedules"}>Back to List</Link>
                </div>
            </main>
        </div>
    )
}

export default ScheduleDetails