function IsDateOfBirthValid(value) {
    
    let dateOfBirth = value.getTime();
    if (Number.isNaN(dateOfBirth)) {
        return false;
    }
    else {
        return true;
    }
}