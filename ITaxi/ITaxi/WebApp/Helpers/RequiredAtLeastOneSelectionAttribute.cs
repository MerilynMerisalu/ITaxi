using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Helpers;

public class RequiredAtLeastOneSelectionAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        bool isValid = false;
        if (value is IEnumerable enumerable)
        {
            isValid = enumerable.GetEnumerator().MoveNext();
        }
        return isValid;
    }
}