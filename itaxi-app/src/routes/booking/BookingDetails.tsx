import React, { useContext, useEffect, useState } from 'react'
import { useParams } from 'react-router-dom';
import { JwtContext } from '../Root';
import { IBooking } from '../../domain/IBooking';
import { BookingService } from '../../services/BookingService';
import { Link } from 'react-router-dom';


const BookingDetails = () => {
  
    const { id } = useParams();
    const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState<IBooking | null>(null)
    const { language } = useContext(JwtContext)
    const bookingService = new BookingService();
    console.log('data test:', data)
    useEffect(() => {
        console.log('jwtloginresponse', jwtLoginResponse)
        if (jwtLoginResponse) {
            bookingService.details(id)
                .then(
                    response => {
                        console.log(`Booking: ${response}`)
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
    <div  className="container">
    <main  role="main" className="pb-3">
        

<h1>Details</h1>

<div>
    <h4>Booking</h4>
    <hr/>
    
<dl className="row">
    <dt className="col-sm-2">
        Vehicle Type
    </dt>
    <dd className="col-sm-10">
        {data?.vehicleType.vehicleTypeName}
    </dd>
    <dt className="col-sm-2">
        City
    </dt>
    <dd className="col-sm-10">
        {data?.city.cityName}
    </dd>
    <dt className="col-sm-2">
        Pick Up Date and Time
    </dt>
    <dd className="col-sm-10">
        {formatDate(data?.pickUpDateAndTime ?? "")}
    </dd>
    <dt className="col-sm-2">
        Pick Up Address 
    </dt>
    <dd className="col-sm-10">
        {data?.pickupAddress}
    </dd>
    <dt className="col-sm-2">
        Destination Address
    </dt>
    <dd className="col-sm-10">
        {data?.destinationAddress}
    </dd>
    <dt className="col-sm-2">
        Number of Passengers
    </dt>
    <dd className="col-sm-10">
        {data?.numberOfPassengers}
    </dd>
    <dt className="col-sm-2">
        Has an Assistant?
    </dt>
    <dd className="col-sm-10">
        <input checked={data?.hasAnAssistant} className="check-box" disabled={true} type="checkbox" />
    </dd>
    <dt className="col-sm-2">
        Additional Info
    </dt>
    <dd className="col-sm-10">
      {data?.additionalInfo}
    </dd>
    <dt className="col-sm-2">
        Status of Booking
    </dt>
    <dd className="col-sm-10">
    <td>
        {bookingService.getBookingStatus(data?.statusOfBooking ?? 0)}
         </td>
    </dd>

</dl>
</div>
<div>
    <Link to={"/bookings"}>Back to List</Link>
</div>
    </main>
</div>
  )
  
}

export default BookingDetails