function selecteddateofbirthvalidation(selected_date_of_birth) {
    let date_of_birth = selected_date_of_birth.getTime();
    if (Number.isNaN(date_of_birth)) {
        return false;
    }
    else {
        return true;
    }
}