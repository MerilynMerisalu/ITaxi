using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Helpers;

/// <summary>
/// Validation Attribute to ensure that at least 1 item is selected in a list
/// </summary>
public class RequiredAtLeastOneSelectionAttribute : ValidationAttribute
{
    /// <summary>
    /// Validate that there is at least one Item in the collection
    /// </summary>
    /// <param name="value">The collection to validate</param>
    /// <returns>True if there is at least 1 item in the collection</returns>
    public override bool IsValid(object? value)
    {
        bool isValid = false;
        if (value is IEnumerable enumerable)
        {
            isValid = enumerable.GetEnumerator().MoveNext();
        }
        return isValid;
    }
}