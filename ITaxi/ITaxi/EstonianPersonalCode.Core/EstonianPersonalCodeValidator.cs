
namespace EstonianPersonalCode.Core
{
    public static class EstonianPersonalCodeValidator
    {
        private const int PersonalIdentificationCodeLength = 11;
        private const int LowestFirstDigitValue = 1;
        private const int HighestFirstDigitValue = 8;
        private static readonly DateOnly MinimumDateValueInEstonianPersonalCode = new DateOnly(1800, 01, 01);
        private static readonly int[] FirstWeights = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 };
        private static readonly int[] SecondWeigths = { 3, 4, 5, 6, 7, 8, 9, 1, 2, 3 };

        public static EstonianPersonalCodeValidationResult Validate(string? personalIdentifierCode)
        {

            bool isInvalid = PersonalIdentifierCodeIsEmptyOrWhitespace(personalIdentifierCode);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.Empty);
            isInvalid = IsPersonalIdentifierCodeLengthInvalid(personalIdentifierCode!);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidLength);
            isInvalid = DoesPersonalIdentifierCodeHaveNonDigits(personalIdentifierCode!, out int[] digits);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.ContainsNonDigits);
            int firstDigit = digits.First();
            isInvalid = IsFirstDigitInvalid(firstDigit);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidFirstDigit);
            var encodedSex = GetEncodedSex(firstDigit!);
            string YearPrefix = GetYearPrefix(firstDigit!);
            bool isValid = IsDateValid(personalIdentifierCode!, YearPrefix, out DateOnly parsedDate);
            if (!isValid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidEncodedDate);
            isInvalid = IsDateLessThanTheMinimumDate(parsedDate);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidDate);
            isInvalid = IsDateInFuture(parsedDate);
            if (isInvalid)
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidDate);
            int checkDigit = ComputeCheckDigit(digits, out bool isCalculatedusingFirstWeights);
            int lastDigit = digits.Last();
            isInvalid = IsNotEqualToLastDigitOfPersonalIdentifierCode(lastDigit, checkDigit);
            if (isInvalid)
            {
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidCheckDigit);
            }
            return EstonianPersonalCodeValidationResult.Success(parsedDate, encodedSex);


        }

        public static bool PersonalIdentifierCodeIsEmptyOrWhitespace(string? personalIdentifierCode)
        {
            return string.IsNullOrWhiteSpace(personalIdentifierCode);
        }

        public static bool IsPersonalIdentifierCodeLengthInvalid(string personalIdentifierCode)
        {
            return personalIdentifierCode.Length != PersonalIdentificationCodeLength;

        }

        public static bool DoesPersonalIdentifierCodeHaveNonDigits(string personalIdentifierCode, out int[] digits)
        {
            var result = personalIdentifierCode.All(c => c >= '0' && c <= '9') ? false : true;
            if (!result)
            {
                digits = personalIdentifierCode.Select(d => d - '0').ToArray();
                return result;
            }
            digits = [];
            return result;
        }

        public static bool IsFirstDigitInvalid(int firstDigit)
        {
            return firstDigit < LowestFirstDigitValue || firstDigit > HighestFirstDigitValue;
        }

        public static EncodedSex GetEncodedSex(int firstDigit) => firstDigit % 2 == 0 ? EncodedSex.Female : EncodedSex.Male;

        public static string GetYearPrefix(int firstDigit)
        {
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
        public static bool IsDateInFuture(DateOnly validatedDate, bool isDateOfTodayAllowed = false)
        {
            var todaysDate = DateOnly.FromDateTime(DateTime.Today);
            if (isDateOfTodayAllowed == true)
                return todaysDate < validatedDate;

            return todaysDate <= validatedDate;
        }



        public static int ComputeCheckDigit(int[] digits, out bool IsCalculatedUsingFirstWeights)
        {
            
            int digitsSum = 0;
            for (int i = 0; i < digits.Length - 1; i++)
            {
                digitsSum += FirstWeights[i] * digits[i];
            }
            ;
            int checkDigit = digitsSum % 11;
            if (checkDigit == 10)
            {
                IsCalculatedUsingFirstWeights = false;
                digitsSum = 0;

                for (int i = 0; i < digits.Length - 1; i++)
                {
                    digitsSum += SecondWeigths[i] * digits[i];
                }
            ;
                checkDigit = digitsSum % 11;
                if (checkDigit == 10)
                {
                    checkDigit = 0;
                    return checkDigit;
                }

            }
            else
            {
                IsCalculatedUsingFirstWeights = true;
                
            }
            return checkDigit;

        }

        public static bool IsNotEqualToLastDigitOfPersonalIdentifierCode(int lastDigit, int checkDigit) => lastDigit != checkDigit;
    }
}

