import React, { useContext, useEffect, useState } from 'react'
import { JwtContext } from '../Root'
import { VehicleService } from '../../services/VehicleService';
import { IVehicle } from '../../domain/IVehicle';
import { VehicleAvailability } from '../../utilities/enums';

const VehiclesIndex = () => {

    const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState([] as IVehicle[])
    const vehicleService = new VehicleService();

    useEffect(() => {
        if (jwtLoginResponse) {
            vehicleService.getAll(jwtLoginResponse.token)
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
        }

    }, [jwtLoginResponse]);


    return (
        <div  className="container">
            <main  role="main" className="pb-3">


                <h1>Index</h1>

                <p>
                    <a href="/DriverArea/Vehicles/Create">Create New</a>
                </p>
                <table className="table">
                    <thead>
                        <tr>

                            <th>
                                Vehicle Type
                            </th>
                            <th>
                                Vehicle Mark
                            </th>
                            <th>
                                Vehicle Model
                            </th>
                            <th>
                                Vehicle Plate Number
                            </th>
                            <th>
                                Manufacture Year
                            </th>
                            <th>
                                Number of Seats
                            </th>
                            <th>
                                Vehicle Availability
                            </th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            {data.map(v => (
                                <>
                                <td key={v.vehicleTypeId}>
                                 {v.vehicleType.vehicleTypeName}
                                </td>
                                <td key={v.vehicleMarkId}>
                                 {v.vehicleMark.vehicleMarkName}
                                </td>
                                <td key={v.vehicleModelId}>
                                    {v.vehicleModel.vehicleModelName}
                                </td>
                                <td>
                                    {v.vehiclePlateNumber}
                                </td>
                                <td>
                                     {v.manufactureYear}
                                </td>
                                <td>
                                    {v.numberOfSeats}
                                </td>
                                <td >
                                    {v.vehicleAvailability === 1 ? VehicleAvailability.Available: VehicleAvailability.InAvailable }
                                </td>
                                </>                              
                            ))}
                            
                            
                            <td>
                                <a href="/DriverArea/Vehicles/Edit/ace1042c-5175-4903-72b1-08db546860a1">Edit</a> |
                                <a href="/DriverArea/Vehicles/Details/ace1042c-5175-4903-72b1-08db546860a1">Details</a> |
                                <a href="/DriverArea/Vehicles/Delete/ace1042c-5175-4903-72b1-08db546860a1">Delete</a> |
                                <a href="/DriverArea/Vehicles/Gallery/ace1042c-5175-4903-72b1-08db546860a1">Gallery</a> |
                            </td>
                        </tr>
                    </tbody>
                </table>
            </main>
        </div>
    )
}

export default VehiclesIndex