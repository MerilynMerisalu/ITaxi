import React, { useContext, useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { IDrive } from '../../domain/IDrive'
import { JwtContext } from '../Root'
import axios from 'axios'
import { DriveService } from '../../services/DriveService'

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

  return (
    <div className="container">
    <main role="main" className="pb-3">        

<h1>Index</h1>

<form action="/DriverArea/Drives/SearchByDate" method="post">
    <input type="date" name="search"/>
    <input type="submit" value="Search"/>
</form>
<p>
    <a target="_blank" href="/Drive">Create PDF</a>
</p>

<table className="table">
    <thead>
    <tr>
        {/* <th>
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
        </th> */}

        <th>
            Comment
        </th>           
                {/* <th>
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
                </th> */}
        <th></th>   
    </tr>
    </thead>
    <tbody>
    {data.map(d => (
        <tr key={d.id}>
            {/* <td>
                {d.bookingDTO.scheduleDTO.shiftDurationTime}
            </td>
            <td>
                {d.bookingDTO.customerDTO.appUserDTO.lastAndFirstName}
            </td>
            <td>
                {d.bookingDTO.customerDTO.disabilityTypeDTO.disabilityTypeName}
            </td> 
            <td>
                {d.bookingDTO.PickUpDateAndTime}
            </td>
           <td>
                {d.bookingDTO.cityDTO.cityName}
            </td>
            <td>
                {d.bookingDTO.pickupAddress}
            </td>
            <td>
                {d.bookingDTO.destinationAddress}
            </td> */}
            {/* <td>
                {d?.booking?.vehicleType?.vehicleTypeName}
            </td>
            <td>
                {d.bookingDTO.vehicleDTO.vehicleIdentifier}
            </td>
            <td>
                {d.bookingDTO.numberOfPassengers}
            </td>
            <td>
                {d.bookingDTO.hasAnAssistant}
            </td>
            <td>
            {(() => {
                switch (d.bookingDTO.statusOfBooking) {
                    case 1:     return "Awaiting";
                    case 2:     return "Accepted";
                    case 3:     return "Declined";
                    default:    return "Awaiting";
                    }
            })()}
            </td>
            <td>
            {(() => {
                switch (d.statusOfDrive) {
                    case 1:     return "Awaiting";
                    case 2:     return "Accepted";
                    case 3:     return "Declined";
                    case 4:     return "Started";
                    case 5:     return "Finished";
                    default:    return "Awaiting";
                    }
            })()}
            </td>  */}
            <td>
                {d.comment.commentText}
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
                    <p style={{display: "inline"}}><a href="/DriverArea/Drives/Accept/89014f28-af0c-460b-38f8-08db5d1db589">Accept</a>
                        |</p>
                    <p style={{display: "inline"}}><a href="/DriverArea/Drives/Decline/89014f28-af0c-460b-38f8-08db5d1db589">Decline</a> |</p>

                    <Link to={`/drive/details/${d.id}`}>Details</Link> |

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