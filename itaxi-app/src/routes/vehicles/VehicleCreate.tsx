import React from 'react'
import { Link } from 'react-router-dom'

const VehicleCreate = () => {
    return (
        <div className="container">
            <main role="main" className="pb-3">

                <h1>Create</h1>

                <h4>Vehicle</h4>
                <hr />
                <div className="row">
                    <div className="col-md-4">
                        <form action="/DriverArea/Vehicles/Create" method="post">
                            <div className="text-danger validation-summary-valid"><ul><li style={{ display: "none" }}></li>
                            </ul></div>

                            <div className="form-group">
                                <label className="control-label" html-for="VehicleTypeId">Vehicle Type</label>
                                <select className="form-control" id="VehicleTypeId" name="VehicleTypeId">
                                    <option>Please Select</option>
                                    <option value="fefa19bc-4f87-4a31-a0de-f53fcc1f89ed">Regular</option>
                                    <option value="51444aaa-8e1e-4aaf-b0f9-d4dd10eb7664">Wheelchair</option>
                                </select>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="VehicleMarkId">Vehicle Mark</label>
                                <select className="form-control" id="VehicleMarkId" name="VehicleMarkId">
                                    <option>Please Select</option>
                                    <option value="a6cd7932-64d6-4ae3-fc3a-08db556c391c">Ford</option>
                                    <option value="ead191e5-3bb9-405b-fc39-08db556c391c">Toyota</option>
                                </select>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="VehicleModelId">Vehicle Model</label>
                                <select className="form-control" id="VehicleModelId" name="VehicleModelId">
                                    <option>Please Select</option>
                                    <option value="1b9044ac-8d48-474b-8015-08db556c392e">Focus</option>
                                    <option value="5b85a958-f455-44c6-8014-08db556c392e">Avensis</option>
                                </select>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="VehiclePlateNumber">Vehicle Plate Number</label>
                                <input className="form-control" type="text" id="VehiclePlateNumber" maxLength={25} name="VehiclePlateNumber" value="" />
                                <span className="text-danger field-validation-valid"></span>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="ManufactureYear">Year</label>
                                <select className="form-control" id="ManufactureYear" name="ManufactureYear">
                                    <option>Please Select</option>
                                    <option>2023</option>
                                    <option>2022</option>
                                    <option>2021</option>
                                    <option>2020</option>
                                    <option>2019</option>
                                    <option>2018</option>
                                </select>
                                <span className="text-danger field-validation-valid"></span>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="NumberOfSeats">Number of Seats</label>
                                <input className="form-control" min="2" max="6" type="number" id="NumberOfSeats" name="NumberOfSeats" value="0" /><input name="__Invariant" type="hidden" value="NumberOfSeats" />
                                <span className="text-danger field-validation-valid"></span>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="VehicleAvailability">Vehicle Availability</label>
                                <select className="form-control" id="VehicleAvailability" name="VehicleAvailability">
                                    <option>Please Select</option>
                                    <option value="1">Available</option>
                                    <option value="2">In-Available</option>
                                </select>
                                <span className="text-danger field-validation-valid"></span>
                            </div>
                            <div className="form-group">
                                <input type="submit" value="Create" className="btn btn-primary" />
                            </div>
                            <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0pQAlFx34AR8fQjXmuL4pIHZx8D6fVO86XxC59WOSnIfAqKM8NRzvVGTEd6Qt8wRzTl9i9_h0fkoEwBt0qfGIkzgv969qIR9_q2KHyTjPH4PCyDBsudUKZie3JyyWrQFNxtWwTTmmGEF7C0nwvkpwKZXbfpqFKmrINqV2oarmOAkg" /></form>
                    </div>
                </div>

                <div>
                    <Link to={"/vehicles"}>Back to List</Link>
                </div>
            </main>
        </div>
    )
}

export default VehicleCreate