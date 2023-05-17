import React, { ChangeEvent, useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { VehicleService } from '../../services/VehicleService'

const VehicleCreate = () => {
    const [values, setValues] = useState({
        VehicleMarkId: '',
        VehicleModelId: '',
        numberOfSeats: '2'
    })
    const service = new VehicleService()
    const [manufactureYears, setManufactureYears] = useState<number[]>()
    const [marks, setMarks] = useState<string[]>()
    useEffect(() => {
        async function downloadYears() {
            const years = await service.getManufactureYears()
            setManufactureYears(years)
        }
        // async function downloadMarks() {
        //     const marks = await service.getMarks() // not yet
        //     setMarks(marks)
        // }
        async function download () {
            const promises = [downloadYears()]//, downloadMarks()]
            await Promise.all(promises)
        }
        download()
    }, [])
    console.log('values', values)
    function handleChange(event: ChangeEvent<HTMLSelectElement | HTMLInputElement>) {
        setValues(currentValues => {
            return {
                ...currentValues,
                [event.target.name]: event.target.value
            }
        })
    }
    const markOptions = [
        { value: "a6cd7932-64d6-4ae3-fc3a-08db556c391c", label: 'Ford' },
        { value: "ead191e5-3bb9-405b-fc39-08db556c391c", label: 'Toyota' }
    ]
    const markViews = markOptions.map(option => (
        <option key={option.value} value={option.value}>{option.label}</option>
    ))
    const modelOptions = [
        { value: "1b9044ac-8d48-474b-8015-08db556c392e", label: 'Focus', markId: 'a6cd7932-64d6-4ae3-fc3a-08db556c391c' },
        { value: "5b85a958-f455-44c6-8014-08db556c392e", label: 'Avensis', markId: '"ead191e5-3bb9-405b-fc39-08db556c391c' }
    ]
    const filteredModelOptions = values.VehicleMarkId === ''
        ? modelOptions
        : modelOptions.filter(option => option.markId === values.VehicleMarkId)
    const modelViews = filteredModelOptions.map(option => (
        <option key={option.value} value={option.value}>{option.label}</option>
    ))
    const yearOptionViews = manufactureYears?.map(year=> (
        <option key={year} value={year}>{year}</option>
    ))
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
                                <select
                                    className="form-control"
                                    id="VehicleTypeId"
                                    name="VehicleTypeId"
                                >
                                    <option>Please Select</option>
                                    <option value="fefa19bc-4f87-4a31-a0de-f53fcc1f89ed">Regular</option>
                                    <option value="51444aaa-8e1e-4aaf-b0f9-d4dd10eb7664">Wheelchair</option>
                                </select>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="VehicleMarkId">Vehicle Mark</label>
                                <select
                                    className="form-control"
                                    id="VehicleMarkId"
                                    name="VehicleMarkId"
                                    value={values.VehicleMarkId}
                                    onChange={handleChange}
                                >
                                    <option>Please Select</option>
                                    {markViews}
                                </select>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="VehicleModelId">Vehicle Model</label>
                                <select
                                    className="form-control" 
                                    id="VehicleModelId"
                                    name="VehicleModelId"
                                    value={values.VehicleModelId}
                                    onChange={handleChange}
                                >
                                    <option>Please Select</option>
                                    {modelViews}
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
                                    {yearOptionViews}
                                </select>
                                <span className="text-danger field-validation-valid"></span>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="NumberOfSeats">Number of Seats</label>
                                <input className="form-control" min="2" max="6" type="number" id="NumberOfSeats" name="numberOfSeats" 
                                value={values.numberOfSeats}  onChange={handleChange}/><input name="__Invariant" type="hidden" value="NumberOfSeats" />
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