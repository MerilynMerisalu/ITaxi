function IsDateOfBirthValid(dateToValidate) {
    let dateOfBirth = dateToValidate.getTime();
    if (Number.isNaN(dateOfBirth)) {
        return false;
    }
    else {
        return true;
    }
}