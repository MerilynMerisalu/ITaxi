import  { useContext, useEffect, useState, } from 'react'
import { JwtContext } from '../Root'
import { VehicleService } from '../../services/VehicleService';
import { IVehicle } from '../../domain/IVehicle';


const Vehicles = () => {
  const vehicleService = new VehicleService();
  const { jwtLoginResponse,setJwtLoginResponse } = useContext(JwtContext);

  const [data, setData] = useState([] as IVehicle[]);

  useEffect(() => {
    if (jwtLoginResponse) {
      vehicleService.getAll(jwtLoginResponse.token).then(
        response => {
          console.log(response);
          if (response){
            setData(response);
          } else {
            setData([]);
          }
        }
      );
    } 
  }, [jwtLoginResponse]);

  return (
    <div>Vehicles {data.length}</div>
  );
}

export default Vehicles;