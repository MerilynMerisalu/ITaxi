import React, { useContext, useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom';
import { JwtContext } from '../Root';
import { ISchedule } from '../../domain/ISchedule';
import { ScheduleService } from '../../services/ScheduleService';


const ScheduleDelete = () => {

    const { id } = useParams();
    const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState<ISchedule | null>(null)
    const scheduleService = new ScheduleService();
    console.log('data test:', data)
    useEffect(() => {
        console.log('jwtloginresponse', jwtLoginResponse)
        if (jwtLoginResponse) {
            scheduleService.deleteDetails(id)
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
        <div b-7882z672yd className="container">
            <main b-7882z672yd role="main" className="pb-3">


                <h1>Delete</h1>

                <h3>Are You Sure You Want To Delete This? </h3>
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

                    <form action="/DriverArea/Schedules/Delete/0022fe87-aa46-4f8d-7778-08db5594171b" method="post">
                        <input type="hidden" id="Id" name="Id" value="0022fe87-aa46-4f8d-7778-08db5594171b" />
                        <input type="submit" value="Delete" className="btn btn-danger" /> |
                        <Link to={"/schedule"}>Back to List</Link>
                        <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0pFOPpONt4UD5bH7DbJObG37FJjqJGKXdIKhV6-vreBR3w17vYLmdFiNkEV4lJOxBV7ZoMJ4E2iL7W5CjJIj05QX_e8U7TDz25XRyUHdJj9AHd0Y7Rf49kBH2QcJmYPsJW_P4hzwtzU60Aqch4AQ_JcHhkhJIcmjZ4_DG789B6gTQ" /></form>
                </div>
            </main>
        </div>
    )
}

export default ScheduleDelete