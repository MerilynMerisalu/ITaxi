import React, { useState, ChangeEvent, useEffect, useContext, FormEvent } from 'react'
import { ISchedule } from '../../domain/ISchedule';
import { ScheduleService } from '../../services/ScheduleService';
import { IRideTime } from '../../domain/IRideTime';
import { RideTimeService } from '../../services/RideTimeService';
import axios from 'axios';
import { JwtContext } from '../Root';
import formatDate from '../../utilities/formatDate';

type Props = {}
const scheduleService = new ScheduleService()
const rideTimeService = new RideTimeService()

const CreateRideTime = (props: Props) => {
    const { language } = useContext(JwtContext);
    const [schedules, setSchedules] = useState<ISchedule[]>();
    const [rideTimes, setRideTimes] = useState<IRideTime[]>()
    const [schedule, setSchedule] = useState('')
    const [isTaken, setIsTaken] = useState(false)
    const [selectedRideTimes, setSelectedRideTimes] = useState<string[]>([])
    useEffect(() => {
        async function downloadSchedules() {
            const schedules = await scheduleService.getAll();
            setSchedules(schedules);
        }
        async function downloadRideTimes() {
            const rideTimes = await rideTimeService.getAll();
            setRideTimes(rideTimes);
        }
        async function download() {
            const promises = [
                downloadSchedules(),
                downloadRideTimes(),
            ];
            await Promise.all(promises);
            console.log("download complete");
        }
        download();
        axios.defaults.headers.common["Accept-Language"] = language;
    }, [language]);
    const scheduleOptions = schedules?.map(schedule => {
        return (
            <option key={schedule.id} value={schedule.id}>{schedule.shiftDurationTime}</option>
        )
    })
    const rideTimeOptions = rideTimes?.map(rideTime => {
        const date = formatDate(rideTime.rideDateTime, language)
        return (
            <option key={rideTime.id} value={rideTime.id}>{date}</option>
        )
    })
    console.log('schedule', schedule)
    function handleScheduleChange(event: ChangeEvent<HTMLSelectElement>) {
        console.log('etv', event.target.value)
        setSchedule(event.target.value)
    }
    function handleRideTimeChange (event: ChangeEvent<HTMLSelectElement>) {
        console.log('ride', event.target.selectedOptions)
        const values = Object.values(event.target.selectedOptions).map(option => option.value)
        setSelectedRideTimes(values)
    }
    function handleIsTakenChange () {
        setIsTaken(!isTaken)
    }
    async function handleSubmit (event: FormEvent) {
        event.preventDefault()
        const data = { selectedRideTimes, scheduleId: schedule, isTaken }
        const response = await rideTimeService.create(data)
        console.log('response', response)
    }
    return (
        <div  className="container">
            <main role="main" className="pb-3">
                <div className="row">
                    <div className="col-md-4">
                        <form onSubmit={handleSubmit}>
                            <div className="text-danger validation-summary-valid" data-valmsg-summary="true"><ul>
                                <li style={{ display: "none" }}></li>
                            </ul></div>

                            <div className="form-group">
                                <label className="control-label" htmlFor="ScheduleId">Shift Duration Time</label>
                                <select
                                    className='form-control'
                                    onChange={handleScheduleChange}
                                >
                                    <option>Please Select</option>
                                    {scheduleOptions}
                                </select>
                            </div>
                            <div className="form-group">
                                <label className="control-label" htmlFor="RideTimes">Ride Times</label>
                                <select
                                    multiple
                                    className="form-control"
                                    value={selectedRideTimes}
                                    onChange={handleRideTimeChange}
                                >
                                    {rideTimeOptions}
                                </select>
                                <span className="text-danger field-validation-valid" data-valmsg-for="SelectedRideTimes" data-valmsg-replace="true"></span>
                            </div>
                            <div className="form-group form-check">
                                <label className="form-check-label">
                                    <input
                                        className="form-check-input"
                                        type="checkbox"
                                        checked={isTaken}
                                        onChange={handleIsTakenChange}
                                    /> Is Taken?
                                </label>
                            </div>

                            <div className="form-group">
                                <input type="submit" value="Create" className="btn btn-primary" />
                            </div>
                            <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0oHJQ3rk9TuS1kS9UdbsEbJhaYPugMjkcWLRRsZDMdLcPX9ZfjeDOa5YI6-cX6hPr6n07GLQlHTc7EFQAC211H6bDckrrcxN1UvC_Wi0UJ2TBD3CNukhhus-GSXdm0n6fNIMPukxaKZDDQ-PrTIzQpVjimkJ007Sn3Q_XEjN6NkeQ" /><input name="IsTaken" type="hidden" value="false" /></form>
                    </div>
                </div>


            </main>
        </div>
    )
}

export default CreateRideTime