import React, { useContext, useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { IDrive } from '../../domain/IDrive'
import { JwtContext } from '../Root'
import axios from 'axios'
import { DriveService } from '../../services/DriveService'
import { BookingService } from '../../services/BookingService'

const bookingService = new BookingService();

const driveService = new DriveService();
const DrivesIndex = () => {
    const [data, setData] = useState([] as IDrive[])
    const { language } = useContext(JwtContext)
    console.log('index language', language)
    useEffect(() => {
        axios.defaults.headers.common['Accept-Language'] = language;
        driveService.getAll()
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

    function pad(d: number) {
        const padded = `0${d}`
        return padded.slice(-2)
    }
    console.log('language', language)
    

    function formatDate(iso: string) {
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

                <form action="/DriverArea/Drives/SearchByDate" method="post">
                    <input type="date" name="search" />
                    <input type="submit" value="Search" />
                </form>
                <p>
                    <a target="_blank" href="https://localhost:7026/DriverArea/Drives/Print">Create PDF</a>
                    
                </p>

                <table className="table">
                    <thead>
                        <tr>
                            <th>
                                Schedule
                            </th>
                            <th>
                                Customer
                            </th>
                            <th>
                                Disability Type
                            </th>

                            <th>
                                Pick Up Date and Time
                            </th>
                            <th>
                                City
                            </th>
                            <th>
                                Pick Up Address
                            </th>
                            <th>
                                Destination Address
                            </th>
                            <th>
                                Vehicle Type
                            </th>
                            <th>
                                Vehicle
                            </th>
                            <th>
                                Number of Passengers
                            </th>
                            <th>
                                Has an Assistant?
                            </th>
                            <th>
                                Status of Booking
                            </th>
                            <th>
                                Status of Drive
                            </th>

                            <th>
                                Comment
                            </th>
                            <th>
                                Drive Decline Date and Time
                            </th>

                            <th>
                                Drive Acceptance Date and Time
                            </th>
                            <th>
                                Start Date and Time of the Drive
                            </th>
                            <th>
                                Drive End Date and Time
                            </th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {data.map(d => (
                            <tr key={d.id}>
                                <td>
                                    {d.booking.schedule.shiftDurationTime}
                                </td>
                                <td>
                                    {d.booking.customer.appUser.lastAndFirstName}
                                </td>
                                <td>
                                    {d.booking.customer.disabilityType.disabilityTypeName}
                                </td>
                                <td>
                                    {formatDate(d.booking.pickUpDateAndTime)}
                                </td>
                                <td>
                                    {d.booking.city.cityName}
                                </td>
                                <td>
                                    {d.booking.pickupAddress}
                                </td>
                                <td>
                                    {d.booking.destinationAddress}
                                </td>
                                <td>
                                    {d.booking.vehicle.vehicleType.vehicleTypeName}
                                </td>
                                <td>
                                    {d.booking.vehicle.vehicleIdentifier}
                                </td>
                                <td>
                                    {d.booking.numberOfPassengers}
                                </td>
                                <td>
                                    <input checked={d.booking.hasAnAssistant} className="check-box" disabled={true} type="checkbox" />
                                </td>
                                <td>
                                    {bookingService.getBookingStatus(d.booking.statusOfBooking)}
                                </td>

                                <td>
                                    {driveService.getDriveStatus(d.statusOfDrive)}
                                </td>
                                    
                                    
                                <td>
                                    {d.comment?.commentText}
                                </td>

                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>


                                <td>
                                    <Link to={`/drive/accept/${d.id}`}>Accept</Link> |
                                    {/* <p style={{ display: "inline" }}><a href="/DriverArea/Drives/Accept/89014f28-af0c-460b-38f8-08db5d1db589">Accept</a>
                                        |</p>
                                    <p style={{ display: "inline" }}><a href="/DriverArea/Drives/Decline/89014f28-af0c-460b-38f8-08db5d1db589">Decline</a> |</p> */}

                                    <Link to={`/drive/details/${d.id}`}>Details</Link> 

                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </main>
        </div>
    )
}

export default DrivesIndex