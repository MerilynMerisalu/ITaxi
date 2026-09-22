function IsDateOfBirthValid(value) {
    let dateOfBirth = value.getTime();
    if (Number.isNaN(dateOfBirth)) {
        return false;
    }
    else
       return true;
    }


function IsDateOfBirthGreaterThanAllowed(value, isTodaysDateAllowed) {
    const dateOfToday = new Date().setHours(0, 0, 0, 0);
    value = value.setHours(0, 0, 0, 0);
    if (isTodaysDateAllowed === false) {
        if (value < dateOfToday) {
            return true;
        }
        else {
            return false;
        }
            
    }
    else {
        if (isTodaysDateAllowed) {
            if (value <= dateOfToday ) {
                return true;
            }
            return false;
        }
    }

}

function ComputeAge(value) {
    const DATEOFBIRTH = new Date(value);
    const DATEOFTODAY = new Date();
    let age = DATEOFTODAY.getFullYear() - DATEOFBIRTH.getFullYear();
    if ((DATEOFTODAY.getMonth() < DATEOFBIRTH.getMonth())) {
        age = age - 1;
        
    }
    else if ((DATEOFTODAY.getMonth() === DATEOFBIRTH.getMonth()) && DATEOFTODAY.getDate() < DATEOFBIRTH.getDate()) {
        age = age - 1;
        
    }

    let result = ValidateAge(age);
    return result;
}

function ValidateAge(age) {
    const minimumRegistrationAge = 18;

    if (age < minimumRegistrationAge)
        return false;
    else
        return true;

}
