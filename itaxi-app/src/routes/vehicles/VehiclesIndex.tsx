import React, { useContext, useEffect, useState } from 'react'
import { JwtContext } from '../Root'
import { VehicleService } from '../../services/VehicleService';
import { IVehicle } from '../../domain/IVehicle';
import { VehicleAvailability } from '../../utilities/enums';
import { Link, Outlet } from 'react-router-dom';
import axios from 'axios';
const vehicleService = new VehicleService()
const VehiclesIndex = () => {
    const [data, setData] = useState([] as IVehicle[])
    const { language } = useContext(JwtContext)
    console.log('index language', language)
    useEffect(() => {
        axios.defaults.headers.common['Accept-Language'] = language;
        vehicleService.getAll()
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

                <p>
                    <Link to="create">Create New</Link>
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
                                    {v.vehicleAvailability === 1 ? VehicleAvailability.Available : VehicleAvailability.InAvailable}
                                </td>
                                <td>
                                    <Link to={`/vehicle/edit/${v.id}`}>Edit</Link> |
                                    <Link to={`/vehicle/details/${v.id}`}>Details</Link> |
                                    <Link to={`/vehicle/delete/${v.id}`}>Delete</Link> |
                                    <Link to={`/vehicle/gallery/${v.id}`}>Gallery</Link> |
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