async function CountryIdChanged(value) {
    PopulateDropDownList("countyId", [], false);
    PopulateDropDownList("cityId", [], false);
    if (!value) return;
    const response = await fetch(`?handler=SetDropDownCountiesList&countryId=${value}`);
    if (!response.ok) {
        console.error("Fetching counties failed!");
        return;
    }

    const data = await response.json();
    PopulateDropDownList("countyId", data, false);
}
async function CountyIdChanged(value) {
    PopulateDropDownList("cityId", [], false);
    const citySelect = document.getElementById('cityId')
    if (!value) return;
    const response = await fetch(`?handler=SetDropDownCitiesList&countyId=${value}`);
    if (!response.ok) {
        console.error("Fetching cities failed!");
        return;
    }

    const data = await response.json();
    PopulateDropDownList("cityId", data, false);
}

function BirthDateValidation(input) {
    const VALUE = input.value;
    const ERRORMESSAGE = input.dataset.errorMessage;
    let dateOfToday = new Date();
    dateOfToday.setHours(0, 0, 0, 0);

    const DATEOFBIRTHERRORSPAN = document.getElementById("error-display");

    let dateOfBirth = new Date(VALUE);
    dateOfBirth.setHours(0, 0, 0, 0);
   
    if (dateOfBirth >= dateOfToday) {
        DATEOFBIRTHERRORSPAN.textContent = input.dataset.errorMessage;
    }
    else {
        DATEOFBIRTHERRORSPAN.textContent = "";
        CalculateAge(dateOfBirth, dateOfToday, input);
    }
}

function CalculateAge(dateOfBirth, dateOfToday, input) {
    let age = dateOfToday.getFullYear() - dateOfBirth.getFullYear();

    const birthdayHasNotOccurred =
        dateOfBirth.getMonth() > dateOfToday.getMonth() ||
        (
            dateOfBirth.getMonth() === dateOfToday.getMonth() &&
            dateOfBirth.getDate() > dateOfToday.getDate()
        );

    if (birthdayHasNotOccurred) {
        age--;
    }

    let result = ValidateAge(age);
    if (result !== true)
        ageErrorSpan.textContent = input.dataset.ageErrorMessage;
    else
        ageErrorSpan.textContent = "";
}

function ValidatePersonalIdentifierNumber(input) {
    
    const PERSONALIDENTIFIERNUMBERERRORSPAN = document.getElementById('personal-identifier-error');
    const FIELD_NAME = input.labels[0].textContent.trim();
    const VALUE = input.value;
    const REQUIRED_ERROR_MESSAGE = input.dataset.errorMessage;
    REQUIRED_ERROR_MESSAGE.replace("{0}", FIELD_NAME);
    const STRING_LENGTH_ERROR_MESSAGE = input.dataset.lengthErrorMessage;
    const GENDER_MISMATCH_ERROR_MESSAGE = input.dataset.genderMismatchErrorMessage;
    const STRING_LENGTH_MIN = input.minLength;
    const STRING_LENGTH_MAX = input.maxLength;
    let result = IsNotEmpty(VALUE);
    if (result !== true) {
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = REQUIRED_ERROR_MESSAGE;
    }
     result = ValidatePersonalIdentifierLength(VALUE);
    if (result !== true) {
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent =
            STRING_LENGTH_ERROR_MESSAGE.replace("{0}", FIELD_NAME)
                .replace("{1}", STRING_LENGTH_MIN)
                .replace("{2}", STRING_LENGTH_MAX);
        return PERSONALIDENTIFIERNUMBERERRORSPAN;

    }
    else {
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
    }
    const GENDER = Number.parseInt(document.getElementById('gender-value').value);
    const PERSONALIDENTIFIERFIRSTDIGIT = Number.parseInt(VALUE[0]);
    result = ValidateGenderBasedOnPersonalIdentifier(GENDER, PERSONALIDENTIFIERFIRSTDIGIT);
    if (result !== true) {
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
        return PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = GENDER_MISMATCH_ERROR_MESSAGE;
    }
    else {
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
    }
    const BIRTHYEARBASE = GetBirthYearBase(PERSONALIDENTIFIERFIRSTDIGIT);


    
}

function ValidateGenderBasedOnPersonalIdentifier(GENDER, PERSONALIDENTIFIERFIRSTDIGIT) {
    if (GENDER === 2) {
        if (PERSONALIDENTIFIERFIRSTDIGIT % 2 !== 0)
            return false;
        else
            return true

    }
    else if (GENDER === 3) {
        if (PERSONALIDENTIFIERFIRSTDIGIT % 2 !== 1)
            return false;
        else
            return true;

    }

    else
        return true;
}
function GetBirthYearBase(personalIdentifierFirstDigit) {
    switch (personalIdentifierFirstDigit) {
        case 1:
        case 2:
            return 1800;
        case 3:
        case 4:
            return 1900;
        case 5:
        case 6:
            return 2000;
        case 7:
        case 8:
            return 2100;
        default:
            return null;
    }
}

function ValidatePersonalIdentifierLength(personalIdentifier) {
    if (personalIdentifier.length !== 11 )
        return false;
    return true;
}

function ValidateAge(age) {
    const minimumRegistrationAge = 18;
    
    if (age < minimumRegistrationAge) 
        return false;
    else
        return true;
    

}

function IsNotEmpty(value) {
    if (!value || value === "") {
        return false; 
    }
    return true;
}



