import React from 'react'

type Props = {}

const RideTimeCreate = (props: Props) => {
  return (
    <div className="container">
      <main role="main" className="pb-3">
        <h1>Create</h1>

        <h4>Ride Time</h4>
        <hr />
        <div className="row">
          <div className="col-md-4">
            <form action="/DriverArea/RideTimes/Create" method="post">
              <div className="text-danger validation-summary-valid" ><ul><li
                style={{ display: "none" }}></li>
              </ul></div>

              <div className="form-group">
                <label className="control-label" html-for="ScheduleId">Shift Duration Time</label>
                <select className="form-control" id="ScheduleId" name="ScheduleId">
                  <option>Please Select</option>

                </select>
              </div>
              <div className="form-group">
                <label className="control-label" html-for="RideTimes">Ride Times</label>
                <select multiple={true} className="form-control" id="SelectedRideTimes" name="SelectedRideTimes">
                </select>

              </div>
              <div className="form-group form-check">
                <label className="form-check-label">
                  <input className="form-check-input" type="checkbox" data-val="true" data-val-required="The Is Taken? field is required." id="IsTaken" name="IsTaken" value="true" /> Is Taken?
                </label>
              </div>

              <div className="form-group">
                <input type="submit" value="Create" className="btn btn-primary" />
              </div>
              <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0pEnI7n-oUrknx4l60HB27nByvrgpINHWN29Nacdmj_PwrrkVJyXRiXUKW9cVbOgbmMoBQrvUGnR3EG7OYCXuPFAdNvp8VwwPmCiraaddFQKBsnoXVENQwZt7pNRLxH-kPvhxVMzK7tgA1DHyMzbH7kN2fREueOeyeksS8LY8ltZw" /><input name="IsTaken" type="hidden" value="false" /></form>
          </div>
        </div>


      </main>
    </div>
  )
}

export default RideTimeCreate