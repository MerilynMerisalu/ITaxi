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

    const minimumRegistrationAge = 18;
    const ageErrorSpan = document.getElementById("error-display");

    if (age < minimumRegistrationAge) {
        ageErrorSpan.textContent = input.dataset.ageErrorMessage;
    } else {
        ageErrorSpan.textContent = "";
    }
}

function ValidatePersonalIdentifierNumber(input) {
    const PERSONALIDENTIFIERNUMBERERRORSPAN = document.getElementById('personal-identifier-error');
    const VALUE = input.value;
    const GENDER = Number.parseInt(document.getElementById('gender-value').value);
    const PERSONALIDENTIFIERFIRSTDIGIT = VALUE[0];
    const BIRTHYEARDIGITS = VALUE.substring(1, 3);
    const BIRTHYEARBASE = GetBirthYearBase(PERSONALIDENTIFIERFIRSTDIGIT);
    console.log(BIRTHYEARBASE);


    if (GENDER === 2) {
        if (PERSONALIDENTIFIERFIRSTDIGIT % 2 !== 0) {
            PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
            PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "The first digit of a personal identifier number does not match the selected gender";
        }
        else {
            PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
        }
    }
    else if (GENDER === 3) {
        if (PERSONALIDENTIFIERFIRSTDIGIT % 2 !== 1) {
            PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
            PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "The first digit of a personal identifier number does not match the selected gender";
        }
        else {
            PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
        }
    }
    else {
        PERSONALIDENTIFIERNUMBERERRORSPAN.textContent = "";
    }
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





