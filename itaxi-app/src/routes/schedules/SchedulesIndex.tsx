import React, { useContext, useEffect, useState } from 'react'
import { JwtContext } from '../Root';
import { ScheduleService } from '../../services/ScheduleService';
import { IScheduleData } from '../../dto/IScheduleData';
import { ISchedule } from '../../domain/ISchedule';

const SchedulesIndex = () => {
    const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState([] as ISchedule[])
    const scheduleService = new ScheduleService();

    useEffect(() => {
        if (jwtLoginResponse) {
            scheduleService.getAll(jwtLoginResponse.token)
                .then(
                    response => {
                        console.log(response)
                        if (response)
                            setData(response)
                        else {
                            setData([])
                        }
                    }
                )
        }

    }, [jwtLoginResponse]);

    return (
        <div className="container">
            <main role="main" className="pb-3">

                <h1>Index</h1>

                <p>
                    <a href="/DriverArea/Schedules/Create">Create New</a>
                </p>
                <table className="table">
                    <thead>
                        <tr>

                            <th>
                                Vehicle
                            </th>
                            <th>
                                Shift Start Date and Time
                            </th>
                            <th>
                                Shift End Date and Time
                            </th>

                            <th>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                        {data.map(s => (
                                <>
                                <td key={s.vehicleId}>
                                 {s.vehicle.vehicleIdentifier}
                                </td>
                                <td>
                                 {s.startDateAndTime}
                                </td>
                                <td>
                                    {s.endDateAndTime}
                                </td>
                                </>                              
                            ))}

                            <td>
                                <a href="/DriverArea/Schedules/Edit/6cfd35ef-ae8b-4cd9-f572-08db5495e718">Edit</a> |
                                <a href="/DriverArea/Schedules/Details/6cfd35ef-ae8b-4cd9-f572-08db5495e718">Details</a> |
                                <a href="/DriverArea/Schedules/Delete/6cfd35ef-ae8b-4cd9-f572-08db5495e718">Delete</a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </main>
        </div>
    )
}

export default SchedulesIndex