import React, { useContext, useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom';
import { JwtContext } from '../Root';
import { VehicleService } from '../../services/VehicleService';
import { IVehicle } from '../../domain/IVehicle';
import { VehicleAvailability } from '../../utilities/enums';
import { Link } from 'react-router-dom';

const VehicleDelete = () => {
    const { id } = useParams();
    const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState<IVehicle | null>(null)
    const vehicleService = new VehicleService();
    const navigate = useNavigate()
    console.log('data test:', data)
    useEffect(() => {
        console.log('jwtloginresponse', jwtLoginResponse)
        if (jwtLoginResponse) {
            vehicleService.deleteDetails(id)
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
    const deleteAction = async () =>{
        const status = await vehicleService.delete(id)
        if (status === 200) {
            navigate('/vehicles')
        } else {
            console.warn('Vehicle delete not OK', status)
        }
    }

    return (
        <div className="container">
            <main role="main" className="pb-3">

                <h1>Delete</h1>

                <h3>Are You Sure You Want To Delete This? </h3>
                <div>
                    <h4>Vehicle</h4>
                    <hr />

                    <dl className="row">

                        <dt className="col-sm-2">
                            Vehicle Type
                        </dt>
                        <dd className="col-sm-10">
                            {data?.vehicleType?.vehicleTypeName}
                        </dd>
                        <dt className="col-sm-2">
                            Vehicle Mark
                        </dt>
                        <dd className="col-sm-10">
                            {data?.vehicleMark?.vehicleMarkName}
                        </dd>
                        <dt className="col-sm-2">
                            Vehicle Model
                        </dt>
                        <dd className="col-sm-10">
                            {data?.vehicleModel?.vehicleModelName}
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

                    <form action="/DriverArea/Vehicles/Delete/65c41561-8581-4ffe-7cae-08db556c394f" method="post">
                        <input type="hidden" id="Id" name="Id" value="65c41561-8581-4ffe-7cae-08db556c394f" />
                        <input type="submit" onClick={deleteAction} value="Delete" className="btn btn-danger" /> |
                        <Link to={"/vehicles"}>Back to List</Link>
                        <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0q8KLw9xhC6hjgoURA-dd0n3oP-GTbcky6oIi8FSBCzGyqGXaMU1uc7jOGed8rHjgtz-fCnATkMZvOq_b6Ne7cH0J5ptrNlkObVyA9Jp2hQ7sg4C22O3cfEDZs54JLeqmddI77XZPVT1seyLOSSrl-mVAoALwW5h9uTQ-y66-ZtKQ" />
                    </form>
                </div>
            </main>
        </div>
    )
}

export default VehicleDelete