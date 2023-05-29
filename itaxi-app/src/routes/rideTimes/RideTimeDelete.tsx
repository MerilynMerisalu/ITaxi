import React, { FormEvent, useContext, useEffect, useState } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom';
import { JwtContext } from '../Root';
import { IRideTime } from '../../domain/IRideTime';
import { RideTimeService } from '../../services/RideTimeService';


const RideTimeDelete = () => {
    const { id } = useParams();
    const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState<IRideTime | null>(null)
    const { language } = useContext(JwtContext)
    const rideTimeService = new RideTimeService();
    const navigate = useNavigate()
    console.log('data test:', data)
    useEffect(() => {
        console.log('jwtloginresponse', jwtLoginResponse)
        if (jwtLoginResponse) {
            rideTimeService.deleteDetails(id)
                .then(
                    response => {
                        console.log(`RideTimes: ${response}`)
                        if (response)
                            setData(response)
                        else {
                            setData(null)
                        }
                    }
                )
        }

    }, [id, jwtLoginResponse, rideTimeService]);

    function pad (r: number) {
        const padded = `0${r}`
        return padded.slice(-2)
    }
    console.log('language', language)

    function formatDate (iso: string) {
        const date = new Date(iso)
        const year = pad(date.getFullYear())
        const month = pad(date.getMonth() + 1)
        const day = pad(date.getDate())
        const hours = pad(date.getHours())
        const minutes = pad(date.getMinutes())

        if (language === 'en-GB') {
            return `${year}-${month}-${day} ${hours}:${minutes}`
        }
        if (language === 'et') {
            return `${day}.${month}.${year} ${hours}:${minutes}`
        }

    }

    const deleteAction = async (event: FormEvent) =>{
        event.preventDefault()
        console.log('deleteAction id test:', id)
        const status = await rideTimeService.delete(id)
        console.log('deleteAction status:', status)
        if (status === 204 || status === 200) {
            console.log('status ok')
            navigate('/ridetimes')
        } else {
            console.warn('Schedule delete not OK', status)
        }
    }
  return (
    <div className="container">
    <main role="main" className="pb-3">    

<h1>Delete</h1>

<h3>Are You Sure You Want To Delete This? </h3>
<div>
    <h4>Ride Time</h4>
    <hr/>
    
<dl className="row">

    <dt className="col-sm-2">
        Shift Duration Time
    </dt>
    <dd className="col-sm-10">
        {data?.schedule.shiftDurationTime??""}
    </dd>
    <dt className="col-sm-2">
        Ride Time
    </dt>
    <dd className="col-sm-10">
        {formatDate(data?.rideDateTime??"")}
    </dd>
    <dt className="col-sm-2">
        Is Taken?
    </dt>
    <dd className="col-sm-10">
        <input checked={data?.isTaken} className="check-box" disabled={true} type="checkbox" />
    </dd>

</dl>

    <form onSubmit={deleteAction}>
        <input type="hidden" id="Id" name="id" value={data?.id} />
        <input type="submit" value="Delete" className="btn btn-danger"/> |
        <Link to={"/rideTimes"}>Back to List</Link>
    <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0pDnV-yYSrMvgy6Q3-z9B11qzvajTpCbUTjFdkkuz9QHavQpAiLrRTEgnkjT_clW8NfwlDT7o0H_KQ9PLY4cDie77nEx4NEXXU3VeG6c7MNxttPbjLZXwriiQxBYsmnR_wkdtfdSq978mjbZT-zQU02ebLk0vBfKASiBWAnfr20PA" /></form>
</div>
    </main>
</div>
  )
}

export default RideTimeDelete