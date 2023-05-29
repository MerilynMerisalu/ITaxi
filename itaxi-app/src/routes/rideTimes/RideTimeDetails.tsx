import React, { useContext, useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom';
import { JwtContext } from '../Root';
import { IRideTime } from '../../domain/IRideTime';
import { RideTimeService } from '../../services/RideTimeService';


const RideTimeDetails = () => {
    const { id } = useParams();
    const { jwtLoginResponse, setJwtLoginResponse } = useContext(JwtContext);
    const [data, setData] = useState<IRideTime | null>(null)
    const { language } = useContext(JwtContext)
    const rideTimeService = new RideTimeService();
    console.log('data test:', data)
    useEffect(() => {
        console.log('jwtloginresponse', jwtLoginResponse)
        if (jwtLoginResponse) {
            rideTimeService.details(id)
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

    }, [id, language]);

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

  return (
    <div className="container">
    <main role="main" className="pb-3">
        
<h1>Details</h1>

<div>
    <h4>Ride Time</h4>
    <hr/>
    
<dl className="row">

    <dt className="col-sm-2">
        Shift Duration Time
    </dt>
    <dd className="col-sm-10">
        {data?.schedule.shiftDurationTime}
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

    <form action="/DriverArea/RideTimes/Details/7085b7cf-d6d7-4cc0-3d41-08db5ebb35cf" method="post">
        <input type="hidden" id="Id" name="Id" value="7085b7cf-d6d7-4cc0-3d41-08db5ebb35cf" />

        <Link to={"/rideTimes"}>Back to List</Link>
    <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0qHoF1JwctkWRdegcnBpKmpvDnDUrGKusUhwHkeR95upJ6n6oGPxYiIQxrvz27EaIt6BQAd52xZ-MKOlfCHneKkZa5Mxe0JV5DvtFjW1_IZSN43p4-Kos5Vs7hR09IK-hzMyVSfbX_dOw6rfZh_wUa9PpQomAO4xijgTrxS0UHbdQ" /></form>
</div>
    </main>
</div>
  )
}

export default RideTimeDetails