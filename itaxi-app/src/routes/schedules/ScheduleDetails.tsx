import React, { useContext, useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom';
import { JwtContext } from '../Root';
import { ScheduleService } from '../../services/ScheduleService';
import { ISchedule } from '../../domain/ISchedule';


const ScheduleDetails = () => {
    const { id } = useParams();
    const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState<ISchedule | null>(null)
    const { language } = useContext(JwtContext)
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

    }, [id, language]);

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
                            {formatDate(data?.startDateAndTime??"")}
                        </dd>
                        <dt className="col-sm-2">
                            Shift End Date and Time
                        </dt>
                        <dd className="col-sm-10">
                            {formatDate(data?.endDateAndTime??"")}
                        </dd>

                    </dl>
                </div>
                <div>
                    <Link to={"/schedules/edit/:id"}>Edit</Link> |
                    <Link to={"/schedules"}>Back to List</Link>
                </div>
            </main>
        </div>
    )
}

export default ScheduleDetails