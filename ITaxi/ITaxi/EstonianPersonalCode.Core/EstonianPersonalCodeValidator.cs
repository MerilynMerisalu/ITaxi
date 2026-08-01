using System.Globalization;
using System;
namespace EstonianPersonalCode.Core
{
    public static class EstonianPersonalCodeValidator
    {
        private const int PersonalIdentificationCodeLength = 11;
        private const int LowestFirstDigitValue = 1;
        private const int HighestFirstDigitValue = 8;
        private static readonly DateTime _dateOfToday = DateTime.Today;

        public static EstonianPersonalCodeValidationResult Validate(string? personalIdentifierCode)
        {
            bool isInvalid = PersonalIdentifierCodeIsEmptyOrWhitespace(personalIdentifierCode);
            if (isInvalid)
              return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.Empty);
            isInvalid = IsPersonalIdentifierCodeLengthInvalid(personalIdentifierCode);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidLength);
            isInvalid = DoesPersonalIdentierCodeHaveNonDigits(personalIdentifierCode);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.ContainsNonDigits);
            throw new NotImplementedException();


        }

        public static bool PersonalIdentifierCodeIsEmptyOrWhitespace(string? personalIdentifierCode)
        {
            if (string.IsNullOrWhiteSpace(personalIdentifierCode))
            {
                return true;
            }

            return false;
        }

       public static bool IsPersonalIdentifierCodeLengthInvalid(string personalIdentifierCode)
        {
            const int PersonalIdentificationCodeLength = 11;
            if (personalIdentifierCode.Length != PersonalIdentificationCodeLength)
            {
                return true;
            }

            return false;

        }

        public static bool DoesPersonalIdentierCodeHaveNonDigits(string personalIdentifierCode)
        {
            if (personalIdentifierCode.All(c => c >= '0' && c <= '9'))
            {
                return false;
            }
            return true;
        }
    }
}

