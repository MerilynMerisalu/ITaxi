import React, { useContext, useEffect, useState } from 'react'

import {  useParams } from 'react-router-dom'
import { IVehicle } from '../../domain/IVehicle'
import { VehicleService } from '../../services/VehicleService';
import { JwtContext } from '../Root';

const VehicleDetails = () => {
const {id} = useParams();
const {jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
const [data, setData] = useState({} as IVehicle)
const vehicleService = new VehicleService();

useEffect(() => {
    if (jwtLoginResponse) {
        vehicleService.details(id)
            .then(
                response => {
                    console.log(`Vehicle: ${response}`)
                    if (response)
                        setData(response)
                    else {
                        setData({}as IVehicle)
                    }
                }
            )
    }

}, [id]);
  return (
    <div>{JSON.stringify(data)}</div>
  )
}

export default VehicleDetails