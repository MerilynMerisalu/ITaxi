import React, { useState, useEffect, useContext } from 'react'
import { Link } from 'react-router-dom'
import { VehicleService } from '../../services/VehicleService'
import { useParams } from 'react-router-dom'
import { JwtContext } from '../Root';
import { IVehicleFormData } from '../../dto/IVehicleFormData';
import VehicleForm from './VehicleForm';
import { useNavigate } from "react-router-dom";

const service = new VehicleService()

const VehicleEdit = () => {
    const navigate = useNavigate()
    const [data, setData] = useState<IVehicleFormData>({
        vehicleTypeId: "",
        vehicleMarkId: "",
        vehicleModelId: "",
        manufactureYear: "",
        numberOfSeats: "2",
        vehiclePlateNumber: "",
        vehicleAvailability: 0,
    })
    console.log('edit data test:', data)
    const { id } = useParams();
    const { jwtLoginResponse } = useContext(JwtContext);
    useEffect(() => {
        console.log('jwtloginresponse', jwtLoginResponse)
        if (jwtLoginResponse) {
            service.details(id)
                .then(
                    response => {
                        console.log(`Vehicle: ${response}`)
                        if (response) {
                           const formData = {
                            ...response,
                            manufactureYear: String(response.manufactureYear),
                            numberOfSeats: String(response.numberOfSeats)
                           }
                            setData(formData)
                        } else {
                            setData({
                                vehicleTypeId: "",
                                vehicleMarkId: "",
                                vehicleModelId: "",
                                manufactureYear: "",
                                numberOfSeats: "2",
                                vehiclePlateNumber: "",
                                vehicleAvailability: 0,
                            })
                        }
                    }
                )
        }

    }, [id, jwtLoginResponse]);
    const editAction = async (values: IVehicleFormData) =>{
        console.log('values test:', values)
        if (id == null){
            throw new Error('Cannot edit without id')
        }
        const status = await service.edit(id, values)
        console.log('status:', status)
        if (status === 204 || status === 200) {
            console.log('status ok')
            navigate("/vehicles");
        } else {
            console.warn('Vehicle create not OK', status)
        }
    }
    
    return (
        <div className="container">
            <main role="main" className="pb-3">

                <h1>Edit</h1>

                <h4>Vehicle</h4>
                <hr />
                <div className="row">
                    <div className="col-md-4">
                       <VehicleForm action={editAction} initialValues={data} />
                    </div>
                </div>

                <div>
                    <Link to={"/vehicles"}>Back to List</Link>
                </div>


            </main>
        </div>
    )
}

export default VehicleEdit