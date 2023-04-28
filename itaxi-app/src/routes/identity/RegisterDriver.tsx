const RegisterDriver = () => {
  return (
    <form>
    <div  className="container">
      <main  role="main" className="pb-3">
        <h1>Register Driver</h1>

        <div className="row">
          <div className="col-md-4">
            <form
              id="registerForm"
              method="post"
              action="/Identity/Account/RegisterDriver"
            >
              <h2>Register</h2>
              <hr />
              <div
                className="text-danger validation-summary-valid"
                data-valmsg-summary="true"
              >
                <ul>
                  <li style={{display:"none"}}></li>
                </ul>
              </div>

              <div className="form-group">
                <label html-for="Input_Email">Email</label>
                <input
                  className="form-control"
                  alt="Enter Your Email Address Here"
                  placeholder="Enter Your Email Address Here"
                  type="email"
                  data-val="true"
                  data-val-email="The Email field is not a valid e-mail address."
                  data-val-required="The Email field is required."
                  id="Input_Email"
                  name="Input.Email"
                  value=""
                />
                <span
                  className="text-danger field-validation-valid"
                  data-valmsg-for="Input.Email"
                  data-valmsg-replace="true"
                ></span>
              </div>
              <div className="form-group">
                <label html-for="Input_FirstName">First Name</label>
                <input
                  className="form-control"
                  alt="Enter Your First Name Here"
                  placeholder="Enter Your First Name Here"
                  type="text"
                  data-val="true"
                  data-val-length='The First Name must be at least 1 and at max 50 characters long."'
                  data-val-length-max="50"
                  data-val-length-min="1"
                  data-val-maxlength="The field First Name must be a string or array type with a maximum length of &#x27;50&#x27;."
                  data-val-maxlength-max="50"
                  data-val-required="The First Name field is required."
                  id="Input_FirstName"
                  maxLength={50}
                  name="Input.FirstName"
                  value=""
                />
                <span
                  className="text-danger field-validation-valid"
                  data-valmsg-for="Input.FirstName"
                  data-valmsg-replace="true"
                ></span>
              </div>
              <div className="form-group">
                <label html-for="Input_LastName">Last Name</label>
                <input
                  className="form-control"
                  alt="Enter Your Last Name Here"
                  placeholder="Enter Your Last Name Here"
                  type="text"
                  data-val="true"
                  data-val-length='The Last Name must be at least 1 and at max 50 characters long."'
                  data-val-length-max="50"
                  data-val-length-min="1"
                  data-val-maxlength="The field Last Name must be a string or array type with a maximum length of &#x27;50&#x27;."
                  data-val-maxlength-max="50"
                  data-val-required="The Last Name field is required."
                  id="Input_LastName"
                  maxLength={50}
                  name="Input.LastName"
                  value=""
                />
                <span
                  className="text-danger field-validation-valid"
                  data-valmsg-for="Input.LastName"
                  data-valmsg-replace="true"
                ></span>
              </div>
              <div className="form-group">
                <label html-for="Input_Gender">Gender</label>
                <select
                  className="form-control"
                  data-val="true"
                  data-val-required="The Gender field is required."
                  id="Input_Gender"
                  name="Input.Gender"
                >
                  <option>Please Select</option>
                  <option value="1">Custom</option>
                  <option value="2">Female</option>
                  <option value="3">Male</option>
                </select>
                <span
                  className="text-danger field-validation-valid"
                  data-valmsg-for="Input.Gender"
                  data-valmsg-replace="true"
                ></span>
              </div>

              <div className="form-group">
                <label html-for="Input_DateOfBirth">Date of Birth</label>
                <input
                  className="form-control"
                  alt="Enter Your Date of Birth Here"
                  placeholder="Enter Your Date of Birth Here"
                  type="date"
                  data-val="true"
                  data-val-required="The Date of Birth field is required."
                  id="Input_DateOfBirth"
                  name="Input.DateOfBirth"
                  value=""
                />
                <input
                  name="__Invariant"
                  type="hidden"
                  value="Input.DateOfBirth"
                />
                <span
                  className="text-danger field-validation-valid"
                  data-valmsg-for="Input.DateOfBirth"
                  data-valmsg-replace="true"
                ></span>
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
                  data-val="true"
                  data-val-length="The field Personal Identifier must be a string with a maximum length of 50."
                  data-val-length-max="50"
                  id="Input_PersonalIdentifier"
                  maxLength={50}
                  name="Input.PersonalIdentifier"
                  value=""
                />
                <span
                  className="text-danger field-validation-valid"
                  data-valmsg-for="Input.PersonalIdentifier"
                  data-valmsg-replace="true"
                ></span>
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
                  data-val="true"
                  data-val-length="The field Driver License Number must be a string with a maximum length of 25."
                  data-val-length-max="25"
                  data-val-required="The Driver License Number field is required."
                  id="Input_DriverLicenseNumber"
                  maxLength={25}
                  name="Input.DriverLicenseNumber"
                  value=""
                />
                <span
                  className="text-danger field-validation-valid"
                  data-valmsg-for="Input.DriverLicenseNumber"
                  data-valmsg-replace="true"
                ></span>
              </div>
              <div className="form-group">
                <label html-for="Input_DriverAndDriverLicenseCategories">
                  Driver License Categories
                </label>
                <select
                  multiple
                  className="form-control"
                  id="Input_DriverAndDriverLicenseCategories"
                  name="Input.DriverAndDriverLicenseCategories"
                >
                  <option value="7ba6d6f5-f5e0-49d1-f416-08db48374a0b">
                    B2
                  </option>
                </select>
                <span
                  className="text-danger field-validation-valid"
                  data-valmsg-for="Input.DriverAndDriverLicenseCategories"
                  data-valmsg-replace="true"
                ></span>
              </div>
              <div className="form-group">
                <label html-for="Input_ExpiryDate">
                  Driver License&#x27;s Expiry Date
                </label>
                <input
                  className="form-control"
                  alt="Choose the Expiry Date of Your Driver License "
                  type="date"
                  data-val="true"
                  data-val-required="The Driver License&#x27;s Expiry Date field is required."
                  id="Input_ExpiryDate"
                  name="Input.ExpiryDate"
                  value=""
                />
                <input
                  name="__Invariant"
                  type="hidden"
                  value="Input.ExpiryDate"
                />
                <span
                  className="text-danger field-validation-valid"
                  data-valmsg-for="Input.ExpiryDate"
                  data-valmsg-replace="true"
                ></span>
              </div>
              <div className="form-group">
                <label html-for="Input_CityId">City</label>
                <select
                  className="form-control"
                  data-val="true"
                  data-val-required="The City field is required."
                  id="Input_CityId"
                  name="Input.CityId"
                >
                  <option>Please Select</option>
                  <option value="f94299f7-b25a-417b-def2-08db483748fd">
                    Tallinn
                  </option>
                </select>
                <span
                  className="text-danger field-validation-valid"
                  data-valmsg-for="Input.CityId"
                  data-valmsg-replace="true"
                ></span>
              </div>

              <div className="form-group">
                <label html-for="Input_Address">Address of Residence</label>
                <input
                  className="form-control"
                  alt="Enter Your Address of Residence Here"
                  placeholder="Enter Your Address of Residence Here"
                  type="text"
                  data-val="true"
                  data-val-length='The Address of Residence must be at least 2 and at max 72 characters long."'
                  data-val-length-max="72"
                  data-val-length-min="2"
                  data-val-required="The Address of Residence field is required."
                  id="Input_Address"
                  maxLength={72}
                  name="Input.Address"
                  value=""
                />
                <span
                  className="text-danger field-validation-valid"
                  data-valmsg-for="Input.Address"
                  data-valmsg-replace="true"
                ></span>
              </div>

              <div className="form-group">
                <label html-for="Input_PhoneNumber">Phone Number</label>
                <input
                  className="form-control"
                  alt="Enter Your Phone Number Here"
                  placeholder="Enter Your Phone Number Here"
                  type="tel"
                  data-val="true"
                  data-val-length='The Phone Number must be at least 1 and at max 50 characters long."'
                  data-val-length-max="50"
                  data-val-length-min="1"
                  data-val-maxlength="The field Phone Number must be a string with a maximum length of 50."
                  data-val-maxlength-max="50"
                  data-val-required="The Phone Number field is required."
                  id="Input_PhoneNumber"
                  maxLength={50}
                  name="Input.PhoneNumber"
                  value=""
                />
                <span
                  className="text-danger field-validation-valid"
                  data-valmsg-for="Input.PhoneNumber"
                  data-valmsg-replace="true"
                ></span>
              </div>
              <div className="form-group">
                <label html-for="Input_Password">Password</label>
                <input
                  className="form-control"
                  alt="Enter Your Password Here"
                  placeholder="Enter Your Password Here"
                  type="password"
                  data-val="true"
                  data-val-length='The Password must be at least 6 and at max 100 characters long."'
                  data-val-length-max="100"
                  data-val-length-min="6"
                  data-val-required="The Password field is required."
                  id="Input_Password"
                  maxLength={100}
                  name="Input.Password"
                  autoComplete="off"
                />
                <span
                  className="text-danger field-validation-valid"
                  data-valmsg-for="Input.Password"
                  data-valmsg-replace="true"
                ></span>
              </div>
              <div className="form-group">
                <label html-for="Input_ConfirmPassword">Confirm Password</label>
                <input
                  className="form-control"
                  alt="Enter Your Password Confirmation Here"
                  placeholder="Enter Your Password Confirmation Here"
                  type="password"
                  data-val="true"
                  data-val-equalto="The password and confirmation password do not match."
                  data-val-equalto-other="*.Password"
                  data-val-required="The Confirm Password field is required."
                  id="Input_ConfirmPassword"
                  name="Input.ConfirmPassword"
                  autoComplete="off"
                />
                <span
                  className="text-danger field-validation-valid"
                  data-valmsg-for="Input.ConfirmPassword"
                  data-valmsg-replace="true"
                ></span>
              </div>

              <button
                id="registerSubmit"
                type="submit"
                className="w-100 btn btn-lg btn-primary"
              >
                Register
              </button>
              <input
                name="__RequestVerificationToken"
                type="hidden"
                value="CfDJ8CZnHOSo3d5GjFLr2xfxn_7bhCdikjV01ApnuiIu7WQTO41ZCbEN4PpZ_zUb98-mAvERmqb7xk6qVzMu0FZ_6cvqS1Pi3TYzwDssZGrkstgJ_roNzB4i8g9zvImZnWebF-ax-KMn5vjWosQ8yYVvDlY"
              />
            </form>
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
export default RegisterDriver;
