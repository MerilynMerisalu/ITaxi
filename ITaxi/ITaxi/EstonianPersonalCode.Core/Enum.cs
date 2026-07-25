using System;
using System.Collections.Generic;
using System.Text;

namespace EstonianPersonalCode.Core
{
    public enum EncodedSex
    {
        Female = 0,
        Male = 1,
        
    } 

    public enum EstonianPersonalCodeValidationError
    {
        None,
        Empty,
        InvalidLength,
        ContainsNonDigits,
        InvalidFirstDigit,
        InvalidDate,
        InvalidCheckDigit
    }
}
