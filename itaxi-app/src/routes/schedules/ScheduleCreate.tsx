import React from 'react'
import { Link } from 'react-router-dom'
import DateTimePicker from '../../components/DateTimePicker'


const ScheduleCreate = () => {
    return (
        <div className="container">
            <main role="main" className="pb-3">

                <h1>Create</h1>

                <h4>Schedule</h4>
                <hr />
                <div className="row">
                    <div className="col-md-4">
                        <form action="/DriverArea/Schedules/Create" method="post">
                            <div className="text-danger validation-summary-valid"><ul><li style={{ display: "none" }}></li>
                            </ul></div>
                            <div className="form-group">
                                <label className="control-label" html-for="VehicleId">Vehicle</label>
                                <select className="form-control" id="VehicleId" name="VehicleId">
                                    <option>Please Select</option>
                                    <option value="a6a674d3-a762-4f1b-c662-08db5594170a">Toyota Avensis 555 XXZ Regular</option>
                                </select>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="StartDateAndTime">Shift Start Date and Time</label>
                                <DateTimePicker />
                                {/* <input className="form-control" type="datetime-local" id="StartDateAndTime" name="StartDateAndTime" value="" /><input name="__Invariant" type="hidden" value="StartDateAndTime" /> */}
                                <span className="text-danger field-validation-valid"></span>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="EndDateAndTime">Shift End Date and Time</label>
                                {/* <input className="form-control" type="datetime-local" id="EndDateAndTime" name="EndDateAndTime" value="" /><input name="__Invariant" type="hidden" value="EndDateAndTime" /> */}
                                <DateTimePicker />

                                <span className="text-danger field-validation-valid"></span>
                            </div>

                            <div className="form-group">
                                <input type="submit" value="Create" className="btn btn-primary" />
                            </div>
                            <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0qOkkzEhj0KnosSlxhdMwvLbphwX0YS2r0A0tbjOuKPtfmQV0HMqLyTEcN4vAq4IRlo6K_QJeA1eib-H89O8ynWgjpuzmxV3b-JsKEGHyHRNZz3nR1BYLCtaeDKazfzKQdvj55QAiaTw3IjAdRmE1AYZnR4_99PK8QT37Gb7cte0A" /></form>
                    </div>
                </div>

                <div>
                    <Link to={"/schedules"}>Back to List</Link>
                </div>
            </main>
        </div>
    )
}

export default ScheduleCreate