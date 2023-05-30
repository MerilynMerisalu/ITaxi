import React, { useContext, useEffect, useState } from 'react'
import { JwtContext } from '../Root';
import { IBooking } from '../../domain/IBooking';
import axios from 'axios';
import { BookingService } from '../../services/BookingService';
import { Link } from 'react-router-dom';
import { StatusOfBooking } from '../../utilities/enums';

const bookingService = new BookingService();
const BookingIndex = () => {
  const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
  const [data, setData] = useState([] as IBooking[])
  const { language } = useContext(JwtContext)

  useEffect(() => {
      axios.defaults.headers.common['Accept-Language'] = language;
      bookingService.getAll()
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
  
  function pad (b: number) {
      const padded = `0${b}`
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

  console.log('data', data)

  return (
    <div className="container">
    <main role="main" className="pb-3">
        
<h1>Index</h1>

<p>
    <Link to="create">Create New</Link>
</p>
<form action="/CustomerArea/Bookings/SearchByCity" method="post">
    <input type="text" name="search"/>
    <input type="submit" value="Search"/>
</form>
<table className="table">
    <thead>
    <tr>

        <th>
            Driver
        </th>

        <th>
            Vehicle Type
        </th>
        <th>
            Vehicle
        </th>
        <th>
            City
        </th>
        <th>
            Pick Up Date and Time
        </th>
        <th>
            Pick Up Address 
        </th>
        <th>
            Destination Address
        </th>
        <th>
            Number of Passengers
        </th>
        <th>
            Has an Assistant?
        </th>
        <th>
            Additional Info
        </th>
        <th>
            Status of Booking
        </th>
        
        <th></th>
    </tr>
    </thead>
    <tbody>
      {data.map(b => (
        <tr key={b.id}>
            <td>
                {b.driver.appUser.lastAndFirstName}
            </td>
             <td>
                {b.vehicleType.vehicleTypeName}
            </td>
             <td>
                {b.vehicle.vehicleIdentifier}
            </td>
           <td>
            {b.city.cityName}
            </td>
             <td>
                {formatDate(b.pickUpDateAndTime)}
            </td>
            <td>
            {b.pickupAddress}
            </td>
            <td>
                {b.destinationAddress}
            </td>
            <td>
                {b.numberOfPassengers}
            </td>
             <td>
                <input checked={b.hasAnAssistant} className="check-box" disabled={true} type="checkbox" />
            </td> 
            <td>
                {b.additionalInfo}
            </td>
            <td>
            {bookingService.getBookingStatus(b.statusOfBooking)}
            </td>
            {b.statusOfBooking !== 3} 
            <td style={{display: "none"}} >
                
            </td> 
              
            <td>
                <Link to={`/booking/details/${b.id}`}>Details </Link> |
                {b.statusOfBooking !== 3 && <Link to={`/booking/decline/${b.id}`}>Decline{b.statusOfBooking}</Link>}
            </td>
            
        </tr>
        ))}
    </tbody>
</table>
    </main>
</div>
  )
}

export default BookingIndex