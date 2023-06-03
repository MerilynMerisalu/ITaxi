import { FormEvent, useContext, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { IBooking } from "../../domain/IBooking";
import { ScheduleService } from "../../services/ScheduleService";
import { JwtContext } from "../Root";
import ScheduleDelete from "../schedules/ScheduleDelete";
import { BookingService } from "../../services/BookingService";

const bookingService = new BookingService();
const BookingDelete = () => {
    const { id } = useParams();
    const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState<IBooking | null>(null)
    const { language } = useContext(JwtContext)
    
    const navigate = useNavigate()
    console.log('data test:', data)
    useEffect(() => {
        console.log('jwtloginresponse', jwtLoginResponse)
        if (jwtLoginResponse) {
            bookingService.deleteDetails(id)
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

    }, [id, jwtLoginResponse, bookingService]);

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


    const deleteAction = async (event: FormEvent) =>{
        event.preventDefault()
        console.log('deleteAction id test:', id)
        const status = await bookingService.delete(id)
        console.log('deleteAction status:', status)
        if (status === 204 || status === 200) {
            console.log('status ok')
            navigate('/bookings')
        } else {
            console.warn('Booking delete not OK', status)
        }
    }

    return (
        <div className="container">
            <main role="main" className="pb-3">

                <h1>Delete</h1>

                <h3>Are You Sure You Want To Delete This? </h3>
                <div>
                    <h4>Booking</h4>
                    <hr />

                    <dl className="row">
                        <dt className="col-sm-2">
                            Vehicle type
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
                            Pick up date and time
                        </dt>
                        <dd className="col-sm-10">
                            {formatDate(data?.pickUpDateAndTime ??"")}
                        </dd>

                        <dt className="col-sm-2">
                            Pick up address
                        </dt>
                        <dd className="col-sm-10">
                            {data?.pickupAddress}
                        </dd>
                        <dt className="col-sm-2">
                            Destination address
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
                            {data?.hasAnAssistant}
                        </dd>
                        <dt className="col-sm-2">
                            Additional info
                        </dt>
                        <dd className="col-sm-10">
                            {data?.additionalInfo}
                        </dd>
                        <dt className="col-sm-2">
                            Status of Booking
                        </dt>
                        <dd className="col-sm-10">
                            {bookingService.getBookingStatus(data?.statusOfBooking ?? 0 )}
                        </dd>
                        


                    </dl>

                    <form onSubmit={deleteAction}>
                        <input type="hidden" id="Id" name="id" value={data?.id} />
                        <input type="submit" value="Delete" className="btn btn-danger" /> |
                        <Link to={"/bookings"}>Back to List</Link>
                        <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0pFOPpONt4UD5bH7DbJObG37FJjqJGKXdIKhV6-vreBR3w17vYLmdFiNkEV4lJOxBV7ZoMJ4E2iL7W5CjJIj05QX_e8U7TDz25XRyUHdJj9AHd0Y7Rf49kBH2QcJmYPsJW_P4hzwtzU60Aqch4AQ_JcHhkhJIcmjZ4_DG789B6gTQ" />
                    </form>
                </div>
            </main>
        </div>
    )
}

export default BookingDelete