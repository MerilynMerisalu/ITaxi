import React, { FormEvent, useContext, useEffect, useState } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom';
import { JwtContext } from '../Root';
import { IDrive } from '../../domain/IDrive';
import { DriveService } from '../../services/DriveService';
import { BookingService } from '../../services/BookingService';


const DriveAccept = () => {
    const navigate = useNavigate()
    const { id } = useParams();
    const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState<IDrive | null>(null)
    const { language } = useContext(JwtContext)
    const driveService = new DriveService();
    const bookingService = new BookingService();
    console.log('data test:', data)
    useEffect(() => {
        console.log('jwtloginresponse', jwtLoginResponse)
        if (jwtLoginResponse) {
            driveService.acceptDetails(id)
                .then(
                    response => {
                        console.log(`Drive: ${response}`)
                        if (response)
                            setData(response)
                        else {
                            setData(null)
                        }
                    }
                )
        }

    }, [id, jwtLoginResponse, language, driveService]);

    function pad(s: number) {
        const padded = `0${s}`
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
    const acceptAction = async (event: FormEvent) => {
        event.preventDefault()
        console.log('acceptAction id test:', id)
        const status = await driveService.accept(id)
        console.log('acceptAction status:', status)
        if (status === 204 || status === 200) {
            console.log('status ok')
            navigate('/drives')
        } else {
            console.warn('Drive accept not OK', status)
        }
    }

    return (
        <div className="container">
            <main role="main" className="pb-3">

                <h1>Accepting Drive</h1>

                <div>
                    <h4>Drive</h4>
                    <hr />
                    <dl className="row">

                        <dl className="row">
                            <dt className="col-sm-2">
                                Schedule
                            </dt>
                            <dd className="col-sm-10">
                                {formatDate(data?.booking.pickUpDateAndTime ?? "")}
                            </dd>
                            <dt className="col-sm-2">
                                Customer&#x27;s Last and First Name
                            </dt>
                            <dd className="col-sm-10">
                                {data?.booking.customer.appUser.lastAndFirstName}
                            </dd>
                            <dt className="col-sm-2">
                                Pickup Date and Time
                            </dt>
                            <dd className="col-sm-10">
                                {formatDate(data?.booking.pickUpDateAndTime ?? "")}
                            </dd>
                            <dt className="col-sm-2">
                                City
                            </dt>
                            <dd className="col-sm-10">
                                {data?.booking.city.cityName}
                            </dd>

                            <dt className="col-sm-2">
                                Pickup Address
                            </dt>
                            <dd className="col-sm-10">
                                {data?.booking.pickupAddress}
                            </dd>
                            <dt className="col-sm-2">
                                Destination Address
                            </dt>
                            <dd className="col-sm-10">
                                {data?.booking.destinationAddress}
                            </dd>
                            <dt className="col-sm-2">
                                Vehicle Type
                            </dt>
                            <dd className="col-sm-10">
                                {data?.booking.vehicle.vehicleType.vehicleTypeName}
                            </dd>
                            <dt className="col-sm-2">
                                Vehicle
                            </dt>
                            <dd className="col-sm-10">
                                {data?.booking.vehicle.vehicleIdentifier}
                            </dd>
                            <dt className="col-sm-2">
                                Number of Passengers
                            </dt>
                            <dd className="col-sm-10">
                                {data?.booking.numberOfPassengers}
                            </dd>
                            <dt className="col-sm-2">
                                Has an Assistant?
                            </dt>
                            <dd className="col-sm-10">
                                <input checked={data?.booking.hasAnAssistant} className="check-box" disabled={true} type="checkbox" />
                            </dd>
                            <dt className="col-sm-2">
                                Status of Booking
                            </dt>
                            <dd className="col-sm-10">
                                {bookingService.getBookingStatus(data?.booking.statusOfBooking ?? 0)}
                            </dd>
                            <dt className="col-sm-2">
                                Status of Drive
                            </dt>
                            <dd className="col-sm-10">
                                {driveService.getDriveStatus(data?.statusOfDrive ?? 0)}
                            </dd>

                        </dl>
                    </dl>
                    
                </div>
                <div>
                    <form onSubmit={acceptAction}>
                        <input type="hidden" id="Id" name="id" value={data?.id} />
                        <input type="submit" value="Accept" className="btn btn-danger" /> |
                        <Link to={"/drives"}>Decline</Link> |
                        <Link to={"/drives"}>In-Progress</Link> |
                        <Link to={"/drives"}>Finished</Link> |
                        <Link to={"/drives"}>Details</Link> |
                        <Link to={"/drives"}>Back to List</Link>
                        <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0pDnV-yYSrMvgy6Q3-z9B11qzvajTpCbUTjFdkkuz9QHavQpAiLrRTEgnkjT_clW8NfwlDT7o0H_KQ9PLY4cDie77nEx4NEXXU3VeG6c7MNxttPbjLZXwriiQxBYsmnR_wkdtfdSq978mjbZT-zQU02ebLk0vBfKASiBWAnfr20PA" /></form>

                </div>
            </main>
        </div>
    )
}

export default DriveAccept