import React, { useContext, useEffect, useState } from 'react'
import { JwtContext } from '../Root'
import { VehicleService } from '../../services/VehicleService';
import { IVehicle } from '../../domain/IVehicle';
import { VehicleAvailability } from '../../utilities/enums';
import { Link, Outlet } from 'react-router-dom';

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
              {data.map(v => (
                            
                                <tr key={v.id}>
                                <td >
                                 {v.vehicleType.vehicleTypeName}
                                </td>
                                <td>
                                 {v.vehicleMark.vehicleMarkName}
                                </td>
                                <td>
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
                                <td>
                                <Link to="/DriverArea/Vehicles/Edit/ace1042c-5175-4903-72b1-08db546860a1">Edit</Link> |
                                <Link to={`/vehicles/${v.id}`}>Details</Link> |
                                <a href="/DriverArea/Vehicles/Delete/ace1042c-5175-4903-72b1-08db546860a1">Delete</a> |
                                <a href="/DriverArea/Vehicles/Gallery/ace1042c-5175-4903-72b1-08db546860a1">Gallery</a> |
                            </td>
                        </tr>
                                                       
                            ))}
                            
                            
                            
                    </tbody>
                </table>
            </main>
            <Outlet />
        </div>
    )
}

export default VehiclesIndex