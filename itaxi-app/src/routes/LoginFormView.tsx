import React from 'react'
import { ILoginData } from '../dto/ILoginData';
import { MouseEvent } from 'react';
type Props = {
    values: ILoginData;
  
    validationErrors: string[]

    handleChange: (target: 
    EventTarget & HTMLInputElement) => void;

    onSubmit: (event: MouseEvent) => void;
}

const LoginFormView = (props: Props) => {
  return (
    <div className="container">
    <main role="main" className="pb-3">
    <h1>Log In</h1>
    <div className="row">
    <div className="col-md-4">
        <section>
            <form id="account" method="post">
                <hr/>
                
                <h2>Use a local account to log in.</h2>
                <div className="form-floating">
                    <input
                    onChange={(e) => props.handleChange(e.target)} className="form-control" autoComplete="username" aria-required="true" type="email" data-val="true" data-val-email="V&#xE4;li E-post ei ole legaalne e-posti aadress." data-val-required="V&#xE4;li E-post on kohustuslik." id="Input_Email" name="Input.Email" value="" />
                    <label className="form-label" html-for="Input_Email">Email</label>
                </div>
                <div className="form-floating">
                    <input onChange={(e) => props.handleChange(e.target)} className="form-control" autoComplete="current-password" aria-required="true" type="password" data-val="true" data-val-required="V&#xE4;li Parool on kohustuslik." id="Input_Password" name="Input.Password" />
                    <label className="form-label" html-for="Input_Password">Password</label>
                    <span className="text-danger field-validation-valid" data-valmsg-for="Input.Password" data-valmsg-replace="true"></span>
                </div>
                <div>
                    <div className="checkbox">
                        <label className="form-label" html-for="Input_RememberMe">
                            <input className="form-check-input" type="checkbox" data-val="true" data-val-required="The J&#xE4;ta mind meelde? field is required." id="Input_RememberMe" name="Input.RememberMe" value="true" />
                            Remember me?
                        </label>
                    </div>
                </div>
                <div>
                    <button id="login-submit" type="submit" className="w-100 btn btn-lg btn-primary" onClick={(e) => props.onSubmit(e)}>
                        Log in
                    </button>
                </div>
                <div>
                    <p>
                        <a id="forgot-password" href="/Identity/Account/ForgotPassword">
                        Forgot your password?
                        </a>
                    </p>

                    <p>
                        <a id="resend-confirmation" href="/Identity/Account/ResendEmailConfirmation">
                        Resend email confirmation
                        </a>
                    </p>
                </div>
            <input name="__RequestVerificationToken" type="hidden" value="CfDJ8H6gnGQdd_VPhYRnzYmPi0qb6_rbZuCWq_NQOoHG9cDQ1b4cVb1BNRGWUWSrZkia_8Z-0m5wyCM_L3i-KbrqOzOcTX7pA_8tmMKWJ0jUJN6WH5k6smdpUq4q3B-bKwwiJ_fDUzJsX6Ml5Bopkg6ES7w" /><input name="Input.RememberMe" type="hidden" value="false" /></form>
        </section>
    </div>
    <div className="col-md-6 col-md-offset-2">
        <section>
            <h3></h3>
            <hr/>
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
  )
}

export default LoginFormView