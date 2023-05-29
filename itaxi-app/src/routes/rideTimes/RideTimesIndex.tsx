import React, { useContext, useEffect, useState } from 'react'
import { IRideTime } from '../../domain/IRideTime'
import { JwtContext } from '../Root'
import axios from 'axios'
import { RideTimeService } from '../../services/RideTimeService';
import { Link } from 'react-router-dom';

const rideTimeService = new RideTimeService();
const RideTimesIndex = () => {
    const [data, setData] = useState([] as IRideTime[])
    const { language } = useContext(JwtContext)
    console.log('index language', language)
    useEffect(() => {
        axios.defaults.headers.common['Accept-Language'] = language;
        rideTimeService.getAll()
            .then(
                response => {
                    console.log('index response', response)
                    if (response)
                        setData(response)
                    else {
                        setData([])
                    }
                }
            )

    }, [language]);

    function pad (r: number) {
        const padded = `0${r}`
        return padded.slice(-2)
    }

    function formatDate (iso: string) {
        const date = new Date(iso)
        /* const year = pad(date.getFullYear())
        const month = pad(date.getMonth() + 1)
        const day = pad(date.getDate())  */
        const hours = pad(date.getHours())
        const minutes = pad(date.getMinutes())

        if (language === 'en-GB') {
            return `${hours}:${minutes}`
        }
        if (language === 'et') {
            return `${hours}:${minutes}`
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
                                Schedule
                            </th> 
                            <th>
                                Ride Time
                            </th>
                            <th>
                                Is Taken?
                            </th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {data.map(r => (
                            <tr key={r.id}>

                                <td>
                                    {r.schedule.shiftDurationTime}
                                </td> 
                                <td>
                                    {formatDate(r.rideDateTime)}
                                    
                                </td>
                                <td>
                                    <input checked={r.isTaken} className="check-box" disabled={true} type="checkbox" />
                                </td>


                                <td>
                                    <Link to={`/rideTimes/details/${r.id}`}>Details</Link> |
                                    <Link to={`/rideTimes/delete/${r.id}`}>Delete</Link>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </main>
        </div>
    )
}

export default RideTimesIndex

function pad(arg0: number) {
    throw new Error('Function not implemented.');
}
