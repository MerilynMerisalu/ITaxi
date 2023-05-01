
import React, {MouseEvent} from 'react';

interface IProps {
    values: {
        Email: string,
        FirstName: string,
        LastName: string,
        Gender: string,
        DateOfBirth: string,
        DisabilityTypeId: string,
        PhoneNumber: string,
        Password: string,
        ConfirmPassword: string,
    },
    validationErrors: string[]

    handleChange: (target: 
    EventTarget & HTMLInputElement | 
    EventTarget & HTMLSelectElement ) => void;

    onSubmit: (event: MouseEvent) => void;
}


const RegisterCustomerFormView = (props: IProps) => {
    
    return (
        
        <div  className="container">
          <main role="main" className="pb-3">
            <h1>Register Customer</h1>
            <ul style={{display: props.validationErrors.length === 0 ? "none": ''}}>
              <li>{props.validationErrors.length > 0 ? props.validationErrors[0]: ""}</li>
            </ul>
  
            <div className="row">
              <div className="col-md-4">
                <form
                  id="registerForm"
                  method="post"
                  action="/Identity/Account/RegisterCustomer"
                >
                  <h2>Register</h2>
                  <hr />
                  <div
                    className="text-danger validation-summary-valid"
                    data-valmsg-summary="true"
                  >
                    <ul>
                    </ul>
                  </div>
                  <div className="form-group">
                    <label html-for="Input_Email">Email</label>
                    <input
                      onChange={(e) => props.handleChange(e.target)}
                      value={props.values.Email}
                      className="form-control"
                      alt="Enter Your Email Address Here"
                      placeholder="Enter Your Email Address Here"
                      type="email"
                      id="Input_Email"
                      name="Email"
                      />
                    {/* <span
                      className="text-danger field-validation-valid"
                      data-valmsg-for="Input.Email"
                      data-valmsg-replace="true"
                    ></span> */}
                  </div>

                  <div className="form-group">
                    <label html-for="Input_FirstName">First Name</label>
                    <input
                      onChange={(e) => props.handleChange(e.target)}
                      value={props.values.FirstName}
                      className="form-control"
                      alt="Enter Your First Name Here"
                      placeholder="Enter Your First Name Here"
                      type="text"
                      id="Input_FirstName"
                      maxLength={50}
                      name="FirstName"
                    />
                    {/* <span
                      className="text-danger field-validation-valid"
                      data-valmsg-for="Input.FirstName"
                      data-valmsg-replace="true"
                    ></span> */}
                  </div>

                  <div className="form-group">
                    <label html-for="Input_LastName">Last Name</label>
                    <input
                      onChange={(e) => props.handleChange(e.target)}
                      value={props.values.LastName}
                      className="form-control"
                      alt="Enter Your Last Name Here"
                      placeholder="Enter Your Last Name Here"
                      type="text"
                      id="Input_LastName"
                      maxLength={50}
                      name="LastName"  
                    />
                    {/* <span
                      className="text-danger field-validation-valid"
                      data-valmsg-for="Input.LastName"
                      data-valmsg-replace="true"
                    ></span> */}
                  </div>

                  <div className="form-group">
                    <label html-for="Input_Gender">Gender</label>
                    <select
                      className="form-control"
                      id="Input_Gender"
                      name="Gender"
                      onChange={(e) => props.handleChange(e.target)}
                    >
                      <option>Please Select</option>
                      <option value={props.values.Gender}>Custom</option>
                      <option value={props.values.Gender}>Female</option>
                      <option value={props.values.Gender}>Male</option>
                    </select>
                    
                    {/* <span
                      className="text-danger field-validation-valid"
                      data-valmsg-for="Input.Gender"
                      data-valmsg-replace="true"
                    ></span> */}
                  </div>

                  <div className="form-group">
                    <label html-for="Input_DateOfBirth">Date of Birth</label>
                    <input
                      onChange={(e) => props.handleChange(e.target)}
                      value={props.values.DateOfBirth}
                      className="form-control"
                      alt="Enter Your Date of Birth Here"
                      type="date"
                      id="Input_DateOfBirth"
                      name="DateOfBirth"
                    />
                    <input
                      name="__Invariant"
                      type="hidden"
                      value="Input.DateOfBirth"
                    />
                    {/* <span
                      className="text-danger field-validation-valid"
                      data-valmsg-for="Input.DateOfBirth"
                      data-valmsg-replace="true"
                    ></span> */}
                  </div>

                  <div className="form-group">
                    <label html-for="Input_DisabilityTypeId">Disability Type</label>
                    <select
                      className="form-control"
                      id="Input_DisabilityTypeId"
                      name="DisabilityTypeId"
                      onChange={(e) => props.handleChange(e.target)}
                    >
                      <option>Please Select</option>
                      <option value={props.values.DisabilityTypeId}>
                        None
                      </option>
                    </select>
                    {/* <span
                      className="text-danger field-validation-valid"
                      data-valmsg-for="Input.DisabilityTypeId"
                      data-valmsg-replace="true"
                    ></span> */}
                  </div>

                  <div className="form-group">
                    <label html-for="Input_PhoneNumber">Phone Number</label>
                    <input
                      onChange={(e) => props.handleChange(e.target)}
                      value={props.values.PhoneNumber}
                      className="form-control"
                      alt="Enter Your Phone Number Here"
                      placeholder="Enter Your Phone Number Here"
                      type="tel"
                      id="Input_PhoneNumber"
                      maxLength={50}
                      name="PhoneNumber"
                    />
                    {/* <span
                      className="text-danger field-validation-valid"
                      data-valmsg-for="Input.PhoneNumber"
                      data-valmsg-replace="true"
                    ></span> */}
                  </div>

                  <div className="form-group">
                    <label html-for="Input_Password">Password</label>
                    <input
                      onChange={(e) => props.handleChange(e.target)}
                      value={props.values.Password}
                      className="form-control"
                      alt="Enter Your Password Here"
                      placeholder="Enter Your Password Here"
                      type="password"
                      id="Input_Password"
                      maxLength={100}
                      name="Password"
                      autoComplete="off"
                    />
                    {/* <span
                      className="text-danger field-validation-valid"
                      data-valmsg-for="Input.Password"
                      data-valmsg-replace="true"
                    ></span> */}
                  </div>

                  <div className="form-group">
                    <label html-for="Input_ConfirmPassword">Confirm Password</label>
                    <input
                      onChange={(e) => props.handleChange(e.target)}
                      value={props.values.ConfirmPassword}
                      className="form-control"
                      alt="Enter Your Password Confirmation Here"
                      placeholder="Enter Your Password Confirmation Here"
                      type="password"
                      id="Input_ConfirmPassword"
                      name="ConfirmPassword"
                      autoComplete="off" 
                    />
                    {/* <span
                      className="text-danger field-validation-valid"
                      data-valmsg-for="Input.ConfirmPassword"
                      data-valmsg-replace="true"
                    ></span> */}
                  </div>
  
                  <button 
                  
                    id="registerSubmit"
                    className="w-100 btn btn-lg btn-primary"
                    onClick={(e) => props.onSubmit(e)}
                  >
                    Register
                  </button>
                  <input
                    name="__RequestVerificationToken"
                    type="hidden"
                    value="CfDJ8CZnHOSo3d5GjFLr2xfxn_5zANPSDJAbYh29gWxR7XL6MshyQQvr1RRwVeoCEul8GT6ITz9XlijGsCCNP7iYFC3MDm0xjrNTnEIKYgjVcpuU9dm8rdUsAzCi98b8F69whXZ8-jMRQiFL7A5flpXHpBs"
                  />
                </form>
              </div>
              <div className="col-md-6 col-md-offset-2">
                <section>
                  <h3>Use another service to log in.</h3>
                  <hr />
                  <div>
                    <p>
                      There are no external authentication services configured.
                    </p>
                  </div>
                </section>
              </div>
            </div>
          </main>
        </div>
        
        
    );
  };
  
  export default RegisterCustomerFormView;
  