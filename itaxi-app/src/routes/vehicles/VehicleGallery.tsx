import React, { useEffect, useState } from 'react'
import { VehicleService } from '../../services/VehicleService';
import { IVehicle } from '../../domain/IVehicle';
import { Link, useParams } from 'react-router-dom';

const VehicleGallery = () => {
    const { id } = useParams();
    const [data, setData] = useState<IVehicle | null>(null)
    const vehicleService = new VehicleService();

    useEffect(() => {
        vehicleService.gallery(id)
            .then(
                response => {
                    console.log(response)
                    if (response)
                        setData(response)
                    else {
                        setData(null)
                    }
                }
            )

    }, []);

    return (
        <div className="container">
            <main role="main" className="pb-3">

                <h1>Vehicle Name</h1>

                <h3>{data?.vehicleIdentifier}</h3>
                <p>
                    There is no images added yet!
                </p>

                <Link to="#">Upload Images</Link>

                <div>
                    <Link to={"/vehicles"}>Back to List</Link>
                </div>

            </main>
        </div>
    )
}

export default VehicleGallery