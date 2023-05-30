import React, { useContext, useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import { IVehicle } from '../../domain/IVehicle'
import { VehicleService } from '../../services/VehicleService';
import { JwtContext } from '../Root';
import { VehicleAvailability } from '../../utilities/enums';
import { Link } from 'react-router-dom';
import VehiclesIndex from './VehiclesIndex';

const VehicleDetails = () => {
    const { id } = useParams();
    const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState<IVehicle | null>(null)
    const vehicleService = new VehicleService();
    console.log('data test:', data)
    useEffect(() => {
        console.log('jwtloginresponse', jwtLoginResponse)
        if (jwtLoginResponse) {
            vehicleService.details(id)
                .then(
                    response => {
                        console.log(`Vehicle: ${response}`)
                        if (response)
                            setData(response)
                        else {
                            setData(null)
                        }
                    }
                )
        }

    }, [id]);
    
    return (
        <div className="container">
            <main role="main" className="pb-3">


                <h1>Details</h1>
                <div>
                    <h4>Vehicle</h4>
                    <hr />

                    <dl className="row">

                        <dt className="col-sm-2">
                            Vehicle Type
                        </dt>
                        <dd className="col-sm-10">
                            {data?.vehicleType.vehicleTypeName}
                        </dd>
                        <dt className="col-sm-2">
                            Vehicle Mark
                        </dt>
                        <dd className="col-sm-10">
                            {data?.vehicleMark.vehicleMarkName}
                        </dd>
                        <dt className="col-sm-2">
                            Vehicle Model
                        </dt>
                        <dd className="col-sm-10">
                            {data?.vehicleModel.vehicleModelName}
                        </dd>
                        <dt className="col-sm-2">
                            Vehicle Plate Number
                        </dt>
                        <dd className="col-sm-10">
                            {data?.vehiclePlateNumber}
                        </dd>
                        <dt className="col-sm-2">
                            Year
                        </dt>
                        <dd className="col-sm-10">
                            {data?.manufactureYear}
                        </dd>
                        <dt className="col-sm-2">
                            Number of Seats
                        </dt>
                        <dd className="col-sm-10">
                            {data?.numberOfSeats}
                        </dd>
                        <dt className="col-sm-2">
                            Vehicle Availability
                        </dt>
                        <dd className="col-sm-10">
                            {data?.vehicleAvailability === 1 ? VehicleAvailability.Available : VehicleAvailability.InAvailable}
                        </dd>

                    </dl>

                    <form action="/DriverArea/Vehicles/Details/0eed7879-3550-49e8-0517-08db5547dad6" method="post">
                        <input type="hidden" id="Id" name="Id" value={data?.id ?? ''} />

                        <Link to={"/vehicles"}>Back to List</Link>
                        <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0rCSnZ-27ju1SgdOhvNt4Fk7RIn4qoW3cKbXVi_1Yaa6PZ6DcDo-kFxAwFMJ8_qmX78yfqCWtDk2IW1UL8l-kn57sKUAQDNwI44BGSUlpiHoRO-N_UXt4sDZMMpbaP1s0_hlk4sb2kCwZrRjUpis21MqWbkTtIO-aTpHC0CuF-n1A" /></form>
                </div>
            </main>
        </div>
    )
}

export default VehicleDetails