
namespace EstonianPersonalCode.Core
{
    public static class EstonianPersonalCodeValidator
    {
        private const int PersonalIdentificationCodeLength = 11;
        private const int LowestFirstDigitValue = 1;
        private const int HighestFirstDigitValue = 8;
        private static readonly DateOnly MinimumDateValueInEstonianPersonalCode = new DateOnly(1800, 01, 01);
        

        public static EstonianPersonalCodeValidationResult Validate(string? personalIdentifierCode)
        {
            
            bool isInvalid = PersonalIdentifierCodeIsEmptyOrWhitespace(personalIdentifierCode);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.Empty);
            isInvalid = IsPersonalIdentifierCodeLengthInvalid(personalIdentifierCode!);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidLength);
            isInvalid = DoesPersonalIdentifierCodeHaveNonDigits(personalIdentifierCode!);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.ContainsNonDigits);
            isInvalid = IsFirstDigitInvalid(personalIdentifierCode!);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidFirstDigit);
            var encodedSex = GetEncodedSex(personalIdentifierCode!);
            string yearBase = GetYearBase(personalIdentifierCode!);
         bool isValid = IsDateValid(personalIdentifierCode!, yearBase, out DateOnly parsedDate);
            if (!isValid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidDateFormat);
            isInvalid = IsDateLessThanTheMinimumDate(parsedDate);
            if (isInvalid )
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidDate);
            isInvalid = IsDateInFuture(parsedDate);
            if(isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidDate);
            throw new NotImplementedException();


        }

        public static bool PersonalIdentifierCodeIsEmptyOrWhitespace(string? personalIdentifierCode)
        {
            return string.IsNullOrWhiteSpace(personalIdentifierCode);
        }

        public static bool IsPersonalIdentifierCodeLengthInvalid(string personalIdentifierCode)
        {
            return personalIdentifierCode.Length != PersonalIdentificationCodeLength;

        }

        public static bool DoesPersonalIdentifierCodeHaveNonDigits(string personalIdentifierCode)
        {
            return personalIdentifierCode.All(c => c >= '0' && c <= '9') ? false : true;
        }

        public static bool IsFirstDigitInvalid(string personalIdentifierCode)
        {
            int firstDigit = personalIdentifierCode[0] - '0';
            return firstDigit < LowestFirstDigitValue || firstDigit > HighestFirstDigitValue;
        }

        public static EncodedSex GetEncodedSex(string personalIdentifierCode) => (personalIdentifierCode[0] -'0') % 2 == 0 ? EncodedSex.Female : EncodedSex.Male;

        public static string GetYearBase(string personalIdentifierCode)
        {
            int firstDigit = personalIdentifierCode.First() - '0';
            string yearBase;
            switch (firstDigit)
            {
                case 1:
                case 2:
                    yearBase = "18";
                    break;
                case 3:
                case 4:
                    yearBase = "19";
                    break;
                case 5:
                case 6:
                    yearBase = "20";
                    break;
                case 7:
                case 8:
                    yearBase = "21";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(firstDigit));

                    
            }
            return yearBase;
        }

        public static bool IsDateValid(string personalIdentifierCode, string yearBase, out DateOnly validatedDate)
        {
            
            string dob = $"{yearBase}{personalIdentifierCode.AsSpan(1, 2)}-" +
                 $"{personalIdentifierCode.AsSpan(3, 2)}-{personalIdentifierCode.AsSpan(5, 2)}";
            bool isValid = DateOnly.TryParseExact(s: dob, format: "yyyy-MM-dd", out validatedDate);
            return isValid;
            
        }


        public static bool IsDateLessThanTheMinimumDate(DateOnly validatedDate) => validatedDate < MinimumDateValueInEstonianPersonalCode;
        public static bool IsDateInFuture(DateOnly validatedDate, bool isDateOfTodayAllowed = false )
        {
            var todaysDate = DateOnly.FromDateTime(DateTime.Today);
            if (isDateOfTodayAllowed == true)
                return todaysDate < validatedDate;
            
            return todaysDate <= validatedDate;
        }
        
    }
}

