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
            
            bool isInvalid = PersonalIdentifierCodeIsEmptyOrWhitespace(personalIdentifierCode!);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.Empty);
            isInvalid = IsPersonalIdentifierCodeLengthInvalid(personalIdentifierCode!);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidLength);
            isInvalid = DoesPersonalIdentierCodeHaveNonDigits(personalIdentifierCode!);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.ContainsNonDigits);
            isInvalid = IsFirstDigitInvalid(personalIdentifierCode!);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidFirstDigit);
            var encodedSex = GetEncodedSex(personalIdentifierCode!);
            int yearBase = GetYearBase(personalIdentifierCode!);

            throw new NotImplementedException();


        }

        public static bool PersonalIdentifierCodeIsEmptyOrWhitespace(string personalIdentifierCode)
        {
            return string.IsNullOrWhiteSpace(personalIdentifierCode);
        }

        public static bool IsPersonalIdentifierCodeLengthInvalid(string personalIdentifierCode)
        {
            return personalIdentifierCode.Length != PersonalIdentificationCodeLength;

        }

        public static bool DoesPersonalIdentierCodeHaveNonDigits(string personalIdentifierCode)
        {
            return personalIdentifierCode.All(c => c >= '0' && c <= '9') ? false : true;
        }

        public static bool IsFirstDigitInvalid(string personalIdentifierCode)
        {
            return personalIdentifierCode[0] >= LowestFirstDigitValue || personalIdentifierCode[0] <= HighestFirstDigitValue;
        }

        public static EncodedSex GetEncodedSex(string personalIdentifierCode) => personalIdentifierCode[0] % 2 == 0 ? EncodedSex.Female : EncodedSex.Male;

        public static int GetYearBase(string personalIdentifierCode)
        {
            int firstDigit = personalIdentifierCode.First() - '0';
            int yearBase;
            switch (firstDigit)
            {
                case 1:
                case 2:
                    yearBase = 1800;
                    break;
                case 3:
                case 4:
                    yearBase = 1900;
                    break;
                case 5:
                case 6:
                    yearBase = 2000;
                    break;
                case 7:
                case 8:
                    yearBase = 2100;
                    break;
                default:
                    yearBase = 0;
                    break;
                    
            }
            return yearBase;
        }
    }
}

