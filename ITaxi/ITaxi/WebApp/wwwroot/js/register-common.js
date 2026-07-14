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
    const value = input.value;
    const errorMessage = input.dataset.errorMessage;
    let dateOfToday = new Date();
    dateOfToday.setHours(0, 0, 0, 0);

    console.log(dateOfToday);
    let DATEOFBIRTHERRORSPAN = document.getElementById("error-display");
    console.log(DATEOFBIRTHERRORSPAN);

    let dateOfBirth = new Date(value);
    dateOfBirth.setHours(0, 0, 0, 0);
   
    if (dateOfBirth >= dateOfToday) {
        DATEOFBIRTHERRORSPAN.textContent = input.dataset.errorMessage;
    }
    else {
        DATEOFBIRTHERRORSPAN.textContent = "";
    }
    

    
}
