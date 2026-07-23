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
    const AGE_ERROR_DISPLAY_SPAN = document.getElementById("error-display");

    if (result !== true) {
        const AGE_ERROR = input.dataset.ageErrorMessage.replace("{0}", 18);
        AGE_ERROR_DISPLAY_SPAN.textContent = AGE_ERROR;
    }
        
    else
        AGE_ERROR_DISPLAY_SPAN.textContent = "";
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
    const CONTAINS_ONLY_DIGITS_ERROR_MESSAGE = input.dataset.onlyDigitsErrorMessage;
    const CHOOSE_GENDER_ERROR = input.dataset.chooseGenderFirstErrorMessage;
    const ENTER_DOB_ERROR = input.dataset.enterDobFirstErrorMessage;
    const SELECTED_DOB_MISMATCH_PERSONAL_IDENTIFIER_CODE_ERROR_MESSAGE = input.dataset.selectedDateMismatchPersonalIdentifierCodeErrorMessage;
    const CONTROL_DIGIT_ERROR_MESSAGE = input.dataset.invalidPersonalIdenticationCodeErrorMessage;
    const GENDER = Number.parseInt(document.getElementById('gender-value').value);
    if (Number.isNaN(GENDER) === true) {
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = CHOOSE_GENDER_ERROR;
        return PERSONALIDENTIFIERNUMBERERRORSPAN;
    }
    else {
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
    }

    const DATE_OF_BIRTH_VALUE = document.getElementById("date_value").value;
    if (IsNotEmpty(DATE_OF_BIRTH_VALUE) !== true) {
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = ENTER_DOB_ERROR;
        return PERSONALIDENTIFIERNUMBERERRORSPAN;
    }
    else {
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
    }

    let result = IsNotEmpty(VALUE);
    if (result !== true) {
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = REQUIRED_ERROR_MESSAGE;
    }
    result = ContainsOnlyDigits(VALUE);
    if (result !== true) {
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = CONTAINS_ONLY_DIGITS_ERROR_MESSAGE.replace("{0}", FIELD_NAME);
        return PERSONALIDENTIFIERNUMBERERRORSPAN;
    }
    else {
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
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
    const PERSONALIDETIFICATIONDATEOFBIRTH = GetDateOfBirthFromPersonalIdentifierNumber(BIRTHYEARBASE, VALUE);
   
    result = CompareDateOfBirths(PERSONALIDETIFICATIONDATEOFBIRTH, DATE_OF_BIRTH_VALUE);
    if (result !== true) {
        return PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = SELECTED_DOB_MISMATCH_PERSONAL_IDENTIFIER_CODE_ERROR_MESSAGE;
    }
    else {
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
    }

    let computed_control_number = ComputeControlDigit(VALUE);
    result = ValidateControlDigit(VALUE, computed_control_number);
    if (result !== true) {
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = CONTROL_DIGIT_ERROR_MESSAGE;
    }
    else
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = '';
    
    
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

function ContainsOnlyDigits(value) {
    return /^\d+$/.test(value); 
}

function GetDateOfBirthFromPersonalIdentifierNumber(BASE_OF_DATE_OF_BIRTH, DATE_OF_BIRTH) {
    const DATE_OF_BIRTH_TEXT = DATE_OF_BIRTH.toString();
    const BASE_OF_YEAR_PREFIX = BASE_OF_DATE_OF_BIRTH.toString().substring(0, 2);
    const DATE_OF_BIRTH_YEAR_DIGITS = DATE_OF_BIRTH_TEXT.substring(1, 3);
    const YEAR = BASE_OF_YEAR_PREFIX + DATE_OF_BIRTH_YEAR_DIGITS;
    const DATE_OF_BIRTH_MONTH_DIGITS = DATE_OF_BIRTH_TEXT.substring(3, 5);
    const DATE_OF_BIRTH_DAY_DIGITS = DATE_OF_BIRTH_TEXT.substring(5, 7);
    let personal_identifier_date_of_birth = `${YEAR}-${DATE_OF_BIRTH_MONTH_DIGITS}-${DATE_OF_BIRTH_DAY_DIGITS}`;
    return personal_identifier_date_of_birth;

}

function CompareDateOfBirths(PERSONALIDETIFICATIONDATEOFBIRTH, DATE_OF_BIRTH_VALUE) {
    const DATE_OF_BIRTH_VALUE_TEXT = DATE_OF_BIRTH_VALUE.toString();
    return DATE_OF_BIRTH_VALUE_TEXT === PERSONALIDETIFICATIONDATEOFBIRTH;
    }

function ComputeControlDigit(personalIdentificationCode) {
    let sum = 0;
    const FIRST_CHECKSUM_WEIGHTS = [1, 2, 3, 4, 5, 6, 7, 8, 9, 1];
    const SECOND_CHECKSUM_WEIGHTS = [3, 4, 5, 6, 7, 8, 9, 1, 2, 3];
    
    
    for (let i = 0; i < personalIdentificationCode.length - 1; i++) {
        sum += Number.parseInt(personalIdentificationCode.at(i)) * FIRST_CHECKSUM_WEIGHTS[i];
    }
    let computed_control_digit = sum % 11;
    if (computed_control_digit === 10) {
        sum = 0;
        for (let i = 0; i < personalIdentificationCode.length; i++) {
            sum += Number.parseInt(personalIdentificationCode.at(i)) * SECOND_CHECKSUM_WEIGHTS[i];
        }
        computed_control_digit = sum % 11;
        if (computed_control_digit === 10) {
            computed_control_digit = 0;
            return computed_control_digit;
        }
        else
            return computed_control_digit;


    }
    return computed_control_digit;
}

function ValidateControlDigit(personalIdentificationCode, computed_control_number) {
    const PERSONAL_IDENTIFICATIONCODE_CONTROL_DIGIT = personalIdentificationCode.at(-1);
    return Number.parseInt(PERSONAL_IDENTIFICATIONCODE_CONTROL_DIGIT) === computed_control_number;
}
   
    
