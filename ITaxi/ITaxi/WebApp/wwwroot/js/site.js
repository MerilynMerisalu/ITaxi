// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/**
 * Populate a drop down list (<select>) in the current viewport with the Id attribute
 * specified by the dropdownId parameter value, populate with the data elements
 * provided in the dataList argument
 * @param dropdownId {string} The Id of the <select> element to populate
 * @param dataList {array} Array of SelectList elements to populate into the drop down list
 * @param clearAllItems {boolean} Flag to indicate that all items, even the first item (please select) 
 * should be removed when we clear the list of previous entries. Set this to 'true' if the 
 * drop down list you ate targeting does not have a 'Please Select' or default first option.
 * @param selectedValue {any} [Optional] The specific value to select in the list, otherwise the first item will be selected by default
 */
function PopulateDropDownList(dropdownId, dataList, clearAllItems, selectedValue) {
    let ddlElement = document.getElementById(dropdownId);
    while (ddlElement.length > 1)
    {
        ddlElement.remove(1);
    }
    // Only remove the first option if we are told to do so
    if(clearAllItems === true && ddlElement.length > 0)
        ddlElement.remove(0);
    
    dataList.forEach(item => {
        let opt = new Option(item.text, item.value);
        if (selectedValue) {
            if (opt.value === selectedValue)
                opt.selected = true;
        } else if (ddlElement.length === 0) {
            opt.selected = true;
        }
        ddlElement.add(opt)
    })
}