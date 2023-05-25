import React from 'react'
import { Link } from 'react-router-dom'

Link

const DrivesIndex = () => {
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
        <tr>
            <td>
                26/05/2023 00:43 - 26/05/2023 07:43
            </td>
            <td>
                M&#xE4;tas Maarika
            </td>
            <td>
                None
            </td>

            <td>
                26/05/2023 00:43:57
            </td>
            <td>
                Tallinn
            </td>
            <td>
                Suurs&#xF5;jam&#xE4;e 15-2
            </td>
            <td>
                S&#xF5;pruse pst 10
            </td>
            <td>
                Regular
            </td>
            <td>
                Toyota Avensis 555 XXZ Regular
            </td>
            <td>
                2
            </td>
            <td>
                <input checked={true} className="check-box" disabled={true} type="checkbox" />
            </td>
            <td>
                Awaiting for Confirmation
            </td>
            <td>
                Awaiting for Confirmation
            </td>

            <td>
                J&#xE4;in teenusega rahule!
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

               


                <a href="/DriverArea/Drives/Details/89014f28-af0c-460b-38f8-08db5d1db589">Details</a>

            </td>
        </tr>
    </tbody>
</table>
    </main>
</div>
  )
}

export default DrivesIndex