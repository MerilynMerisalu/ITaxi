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


