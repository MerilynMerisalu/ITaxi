import React, { useEffect } from 'react'
import { Link, useParams } from 'react-router-dom'
import ScheduleForm from './ScheduleForm'

const ScheduleEdit = () => {
    const { id } = useParams()
    console.log('*** ID', id)

    return (
        <div className="container">
            <main role="main" className="pb-3">

                <h1>Edit</h1>

                <h4>Schedule</h4>
                <hr />
                <div className="row">
                    <div className="col-md-4">

                        {/* <ScheduleForm id={id} action={async()=> console.log(111)} initialValues={null}/> */}
                        <form action="/DriverArea/Schedules/Edit/d88e5375-314a-4c3d-f505-08db556c3975" method="post">
                            <div className="text-danger validation-summary-valid"><ul><li style={{ display: "none" }}></li>
                            </ul></div>
                            <div className="form-group">
                                <label className="control-label" html-for="VehicleId">Vehicle</label>
                                <select className="form-control" id="VehicleId" name="VehicleId">
                                    <option>Please Select</option>
                                    <option selected={true} value="65c41561-8581-4ffe-7cae-08db556c394f">Toyota Avensis 555 XXZ Regular</option>
                                </select>
                                <span className="text-danger field-validation-valid"></span>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="StartDateAndTime">Shift Start Date and Time</label>
                                <input className="form-control" type="datetime-local" id="StartDateAndTime" name="StartDateAndTime" value="2023-05-16T05:45:00.000" /><input name="__Invariant" type="hidden" value="StartDateAndTime" />
                                <span className="text-danger field-validation-valid"></span>
                            </div>
                            <div className="form-group">
                                <label className="control-label" html-for="EndDateAndTime">Shift End Date and Time</label>
                                <input className="form-control" type="datetime-local" id="EndDateAndTime" name="EndDateAndTime" value="2023-05-16T12:45:00.000" /><input name="__Invariant" type="hidden" value="EndDateAndTime" />
                                <span className="text-danger field-validation-valid"></span>
                            </div>
                            <input type="hidden" id="Id" name="Id" value="d88e5375-314a-4c3d-f505-08db556c3975" />
                            <div className="form-group">
                                <input type="submit" value="Save" className="btn btn-primary" />
                            </div>
                            <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0oyDgXl1QQKFfQ0dCZExmdlQ4GaLKzSjU7TnRl0oKK5_nJJaN7TrhsVwF6_8giCkB2ik3TjAUgLNmw8GFxvamrKtTMV4AhQQ-qoG0t-teZj7MOsVbWmRpv7vM0JR_w54szA1cku2iogS0PbOG8zZLvCTBCMpb8dwHD-vDozT4oReg" /></form>
                    </div>
                </div>

                <div>
                    <Link to={"/schedules"}>Back to List</Link>
                </div>
            </main>
        </div>
    )
}

export default ScheduleEdit