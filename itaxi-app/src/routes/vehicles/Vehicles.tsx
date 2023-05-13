import { useContext, useEffect, useState, } from 'react'
import { JwtContext } from '../Root'
import { VehicleService } from '../../services/VehicleService';
import { IVehicle } from '../../domain/IVehicle';
import { Table } from 'react-bootstrap';
import { VehicleAvailability } from '../../utilities/enums';


const Vehicles = () => {
  const vehicleService = new VehicleService();
  const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);

  const [data, setData] = useState([] as IVehicle[]);
  const [vehicleType, setVehicleType] = useState([])

  useEffect(() => {
    if (jwtLoginResponse) {
      vehicleService.getAll(jwtLoginResponse.token).then(
        response => {
          console.log(response);
          if (response) {
            //var vehicleAvailability: VehicleAvailability = response[0].vehicleAvailability;
          
            setData(response);
          } else {
            setData([]);
          }
        }
      );
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
                Year
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
            {data.map((data => (
              <tr>
              <td>
                
              </td>
              <td>
                
              </td>
              <td>
                
              </td>
              <td>
                {data.vehiclePlateNumber}
              </td>
              <td>
                {data.manufactureYear}
              </td>
              <td>
                {data.numberOfSeats}
              </td>
              <td>
                {data.vehicleAvailability == 1 ? VehicleAvailability.Available : VehicleAvailability.InAvailable}
              </td>


              <td>
                <a href="/DriverArea/Vehicles/Edit/17e8dd1e-b5a5-4d46-717c-08db530b344a">Edit</a> |
                <a href="/DriverArea/Vehicles/Details/17e8dd1e-b5a5-4d46-717c-08db530b344a">Details</a> |
                <a href="/DriverArea/Vehicles/Delete/17e8dd1e-b5a5-4d46-717c-08db530b344a">Delete</a> |
                <a href="/DriverArea/Vehicles/Gallery/17e8dd1e-b5a5-4d46-717c-08db530b344a">Gallery</a> |
              </td>
            </tr>
            )))}
            
          </tbody>
        </table>
      </main>
    </div>
  );
}

export default Vehicles;