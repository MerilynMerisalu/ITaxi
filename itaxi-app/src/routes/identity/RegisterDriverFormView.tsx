import { useEffect, useState } from "react";
import { ICityData } from "../../dto/ICityData";
import { IDriverLicenseCategoryData } from "../../dto/IDriverLicenseCategoryData";
import { IRegisterDriverData } from "../../dto/IRegisterDriverData";
import { DriverLicenseCategoryService } from "../../services/DriverLicenseCategoryService";
import { CityService } from "../../services/CityService";
import { IDriverLicenseCategory } from "../../domain/IDriverLicenseCategory";
import { ICity } from "../../domain/ICity";
import { Gender } from "../../utilities/enums";

const driverLicenseCategoryService = new DriverLicenseCategoryService();
const cityService = new CityService();
const genderEntries = Object.values(Gender).filter(value => isNaN(Number(value)));
console.log('genderEntries', genderEntries)
interface IProps {
  values: IRegisterDriverData;
  cities: ICityData[];
  driverLicenseCategories: IDriverLicenseCategoryData[]

  validationErrors: string[]

  handleChange: (target:
    EventTarget & HTMLInputElement |
    // EventTarget & HTMLOptionsCollection |
    EventTarget & HTMLSelectElement) => void;

  onSubmit: (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
}

const RegisterDriverFormView = (props: IProps) => {
  const [driverLicenseCategories, setDriverLicenseCategories] = useState<IDriverLicenseCategory[]>();
  const [cities, setCities] = useState<ICity[]>();
  useEffect(() => {
    async function downloadDriverLicenseCategories() {
      const driverLicenseCategories = await driverLicenseCategoryService.getAll()
      setDriverLicenseCategories(driverLicenseCategories);
      console.log({ driverLicenseCategories });
    }
    async function downloadCities() {
      const cities = await cityService.getAll();
      setCities(cities);
      console.log({ cities });
    }
    async function download() {
      const promises = [
        downloadDriverLicenseCategories(),
        downloadCities(),

      ];
      await Promise.all(promises);
      console.log("download complete");
    }
    download();

  }, [])
  const genderOptionViews = genderEntries.map((entry, index) => {
    const value = index + 1;
    return (
      <option key={value} value={value}>
        {entry}
      </option>
    );
  });
  console.log("driver cateogires", driverLicenseCategories)
  const driverLicenseOptionViews = driverLicenseCategories?.map((item) => {
    // const value = index + 1;
    return (
      <option key={item.id} value={item.id}>
        {item.driverLicenseCategoryName}
      </option>
    );
  });

  const cityOptionViews = cities?.map((item) => {
    // const value = index + 1;
    return (
      <option key={item.id} value={item.id}>
        {item.cityName}
      </option>
    );
  });
  console.log('props.values', props.values)
  return (
    <form
      id="registerForm"
      method="post"
      action="/Identity/Account/RegisterDriver"
      onSubmit={(e) => e.preventDefault()}>
      <div className="container">
        <main role="main" className="pb-3">
          <h1>Register Driver</h1>

          <div className="row">
            <div className="col-md-4">
              <h2>Register</h2>
              <hr />
              <div
                className="text-danger validation-summary-valid"
                
              >
                <ul>
                  <li style={{ display: "none" }}></li>
                </ul>
              </div>

              <div className="form-group">
                <label html-for="Input_Email">Email</label>
                <input
                  name="Email"
                  className="form-control"
                  alt="Enter Your Email Address Here"
                  placeholder="Enter Your Email Address Here"
                  type="email"
                  value={props.values.Email}
                  onChange={(e) => props.handleChange(e.target)}
                />

              </div>
              <div className="form-group">
                <label html-for="Input_FirstName">First Name</label>
                <input
                  className="form-control"
                  alt="Enter Your First Name Here"
                  placeholder="Enter Your First Name Here"
                  type="text"

                  maxLength={50}
                  name="firstName"
                  value={props.values.firstName}
                  onChange={(e) => props.handleChange(e.target)}
                />

              </div>
              <div className="form-group">
                <label html-for="Input_LastName">Last Name</label>
                <input
                  className="form-control"
                  alt="Enter Your Last Name Here"
                  placeholder="Enter Your Last Name Here"
                  type="text"
                  maxLength={50}
                  name="lastName"
                  value={props.values.lastName}
                  onChange={(e) => props.handleChange(e.target)}
                />

              </div>
              <div className="form-group">
                <label html-for="Input_Gender">Gender</label>
                <select
                  className="form-control"
                  name="Gender"
                  value={props.values.Gender}
                  onChange={(e) => props.handleChange(e.target)}
                >
                  <option>Please Select</option>
                  {genderOptionViews}
                </select>

              </div>

              <div className="form-group">
                <label html-for="Input_DateOfBirth">Date of Birth</label>
                <input
                  className="form-control"
                  alt="Enter Your Date of Birth Here"
                  placeholder="Enter Your Date of Birth Here"
                  type="date"

                  name="DateOfBirth"
                  value={props.values.DateOfBirth}
                  onChange={(e) => props.handleChange(e.target)}
                />
                <input
                  name="__Invariant"
                  type="hidden"
                  value="Input.DateOfBirth"
                />

              </div>

              <div className="form-group">
                <label html-for="Input_PersonalIdentifier">
                  Personal Identifier
                </label>
                <input
                  className="form-control"
                  alt="Enter Your Personal Identifier Code Here"
                  placeholder="Enter Your Personal Identifier Code Here"
                  type="text"
                  maxLength={50}
                  name="PersonalIdentifier"
                  value={props.values.PersonalIdentifier}
                  onChange={(e) => props.handleChange(e.target)}
                />

              </div>
              <div className="form-group">
                <label html-for="Input_DriverLicenseNumber">
                  Driver License Number
                </label>
                <input
                  className="form-control"
                  alt="Enter Your Driver License Number Here"
                  placeholder="Enter Your Driver License Number Here"
                  type="text"

                  name="DriverLicenseNumber"
                  value={props.values.DriverLicenseNumber}
                  onChange={(e) => props.handleChange(e.target)}
                />

              </div>
              <div className="form-group">
                <label html-for="Input_DriverAndDriverLicenseCategories">
                  Driver License Categories
                </label>
                <select
                  multiple={true}
                  className="form-control"
                  name="DriverLicenseCategories"
                  value={props.values.DriverLicenseCategories}
                  onChange={(e) => props.handleChange(e.target)}
                >
                  {driverLicenseOptionViews}

                </select>

              </div>
              <div className="form-group">
                <label html-for="Input_ExpiryDate">
                  Driver License&#x27;s Expiry Date
                </label>
                <input
                  className="form-control"
                  alt="Choose the Expiry Date of Your Driver License "
                  type="date"

                  name="DriverLicenseExpiryDate"
                  value={props.values.DriverLicenseExpiryDate}
                  onChange={(e) => props.handleChange(e.target)}
                />
                <input
                  name="__Invariant"
                  type="hidden"
                  value="Input.ExpiryDate"
                />

              </div>
              <div className="form-group">
                <label html-for="Input_CityId">City</label>
                <select
                  className="form-control"
                  name="CityId"
                  value={props.values.CityId}
                  onChange={e => props.handleChange(e.target)}
                >
                  <option>Please Select</option>
                  {cityOptionViews}

                </select>
              </div>

              <div className="form-group">
                <label html-for="Input_Address">Address of Residence</label>
                <input
                  className="form-control"
                  alt="Enter Your Address of Residence Here"
                  placeholder="Enter Your Address of Residence Here"
                  type="text"
                  maxLength={72}
                  name="Address"
                  value={props.values.Address}
                  onChange={(e) => props.handleChange(e.target)}
                />

              </div>

              <div className="form-group">
                <label html-for="Input_PhoneNumber">Phone Number</label>
                <input
                  className="form-control"
                  alt="Enter Your Phone Number Here"
                  placeholder="Enter Your Phone Number Here"
                  type="tel"
                  maxLength={50}
                  name="PhoneNumber"
                  value={props.values.PhoneNumber}
                  onChange={(e) => props.handleChange(e.target)}
                />

              </div>
              <div className="form-group">
                <label html-for="Input_Password">Password</label>
                <input
                  className="form-control"
                  alt="Enter Your Password Here"
                  placeholder="Enter Your Password Here"
                  type="password"

                  maxLength={100}
                  name="Password"
                  autoComplete="off"
                  value={props.values.Password}
                  onChange={(e) => props.handleChange(e.target)}
                />

              </div>
              <div className="form-group">
                <label html-for="Input_ConfirmPassword">Confirm Password</label>
                <input
                  className="form-control"
                  alt="Enter Your Password Confirmation Here"
                  placeholder="Enter Your Password Confirmation Here"
                  type="password"
                  name="ConfirmPassword"
                  autoComplete="off"
                  value={props.values.ConfirmPassword}
                  onChange={(e) => props.handleChange(e.target)}
                />
              </div>

              <button
                id="registerSubmit"
                onClick={(e) => props.onSubmit(e)}
                className="w-100 btn btn-lg btn-primary"
              >
                Register
              </button>
              <input
                name="__RequestVerificationToken"
                type="hidden"
                value="CfDJ8CZnHOSo3d5GjFLr2xfxn_7bhCdikjV01ApnuiIu7WQTO41ZCbEN4PpZ_zUb98-mAvERmqb7xk6qVzMu0FZ_6cvqS1Pi3TYzwDssZGrkstgJ_roNzB4i8g9zvImZnWebF-ax-KMn5vjWosQ8yYVvDlY"
              />
            </div>
            <div className="col-md-6 col-md-offset-2">
              <section>
                <h3>Use another service to log in.</h3>
                <hr />
                <div>
                  <p>There are no external authentication services configured.</p>
                </div>
              </section>
            </div>
          </div>
        </main>
      </div>
    </form>
  );
};
export default RegisterDriverFormView;
