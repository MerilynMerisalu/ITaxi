import React, { useContext, useEffect, useState } from 'react'
import { JwtContext } from '../Root';
import { ScheduleService } from '../../services/ScheduleService';
import { IScheduleData } from '../../dto/IScheduleData';
import { ISchedule } from '../../domain/ISchedule';
import { Link } from 'react-router-dom';

const SchedulesIndex = () => {
    // const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState([] as ISchedule[])
    const scheduleService = new ScheduleService();

    useEffect(() => {

        scheduleService.getAll()
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

    }, []);

    return (
        <div className="container">
            <main role="main" className="pb-3">

                <h1>Index</h1>

                <p>
                    <Link to="create">Create New</Link>
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

                        {data.map(s => (
                            <tr key={s.vehicleId}>
                                <td>
                                    {s.vehicle.vehicleIdentifier}
                                </td>
                                <td>
                                    {s.startDateAndTime}
                                </td>
                                <td>
                                    {s.endDateAndTime}
                                </td>

                                <td>
                                    <Link to={`/schedules/edit/${s.id}`}>Edit</Link> |
                                    <Link to={`/schedules/details/${s.id}`}>Details</Link> |
                                    <Link to={`/schedules/delete/${s.id}`}>Delete</Link> |
                                </td>
                            </tr>

                        ))}

                    </tbody>
                </table>
            </main>
        </div>
    )
}

export default SchedulesIndex