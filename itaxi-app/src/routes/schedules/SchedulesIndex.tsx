import React, { useContext, useEffect, useState } from 'react'
import { JwtContext } from '../Root';
import { ScheduleService } from '../../services/ScheduleService';
import { IScheduleData } from '../../dto/IScheduleData';
import { ISchedule } from '../../domain/ISchedule';
import { Link } from 'react-router-dom';
import axios from 'axios'
const scheduleService = new ScheduleService();
const SchedulesIndex = () => {
    // const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState([] as ISchedule[])
    const { language } = useContext(JwtContext)

    useEffect(() => {
        axios.defaults.headers.common['Accept-Language'] = language;
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

    }, [language]);
    function pad (s: number) {
        const padded = `0${s}`
        return padded.slice(-2)
    }
    console.log('language', language)

    function formatDate (iso: string) {
        const date = new Date(iso)
        const year = pad(date.getFullYear())
        const month = pad(date.getMonth() + 1)
        const day = pad(date.getDate())
        const hours = pad(date.getHours())
        const minutes = pad(date.getMinutes())

        if (language === 'en-GB') {
            return `${year}-${month}-${day} ${hours}:${minutes}`
        }
        if (language === 'et') {
            return `${day}.${month}.${year} ${hours}:${minutes}`
        }
    }


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
                                    {formatDate(s.startDateAndTime)}
                                </td>
                                <td>
                                    {formatDate(s.endDateAndTime)}
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