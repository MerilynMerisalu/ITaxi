using System.Globalization;
using System;
namespace EstonianPersonalCode.Core
{
    public static class EstonianPersonalCodeValidator
    {
        private const int PersonalIdentificationCodeLength = 11;
        private const int LowestFirstDigitValue = 1;
        private const int HighestFirstDigitValue = 8 ;
        
        public static EstonianPersonalCodeValidationResult Validate(string? personalIdentifierCode)
        {
           
                if (string.IsNullOrWhiteSpace(personalIdentifierCode))
                {
                    return EstonianPersonalCodeValidationResult
                    .Failure(EstonianPersonalCodeValidationError.Empty);
                }
                else if (personalIdentifierCode.Length != PersonalIdentificationCodeLength)
                {
                    return EstonianPersonalCodeValidationResult
                    .Failure(EstonianPersonalCodeValidationError.InvalidLength);
                }
            int[] digits = new int[PersonalIdentificationCodeLength]; 
            for (int i = 0; i < personalIdentifierCode.Length; i++)
            {
                if (int.TryParse(personalIdentifierCode[i].ToString(), out int parsedDigit) == false)
                {
                    return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.ContainsNonDigits);
                }

                digits[i] = parsedDigit;
            }
                int firstDigit = digits[0];
                if (firstDigit < LowestFirstDigitValue || firstDigit > HighestFirstDigitValue)
                {
                    return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidFirstDigit);
                }
                EncodedSex encodedSexValue; 
                if (firstDigit % 2 == 1)
                {
                    encodedSexValue = EncodedSex.Male;
                  
                }
                else
                {
                    encodedSexValue = EncodedSex.Female;
                }

                string birthYearBase;

                switch (firstDigit)
                {
                    case 1:
                    case 2:
                        birthYearBase = "18";
                        break;
                    case 3:
                    case 4:
                        birthYearBase = "19";
                        break;
                    case 5:
                    case 6:
                        birthYearBase = "20";
                        break;
                    case 7:
                    case 8:
                        birthYearBase = "21";
                        break;
                    default:
                        birthYearBase = String.Empty;
                        break;
                }

            string yearDigits = personalIdentifierCode.Substring(startIndex: 1, 2);
            string birthYear = birthYearBase + yearDigits;
            string birthMonth = personalIdentifierCode.Substring(startIndex:3, 2);
            string birthDay = personalIdentifierCode.Substring (startIndex: 5, 2);
            string dateOfBirth = $"{birthYear}-{birthMonth}-{birthDay}";
            bool isValid = DateOnly.TryParseExact(s: dateOfBirth, format: "yyyy-MM-dd", 
                result: out DateOnly parsedDateOfBirth,
                provider: CultureInfo.InvariantCulture, style: DateTimeStyles.None );
            if (isValid == false)
            {
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidDate);
            }

            
            int[] firstChecksumWeights = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 };
            int[] secondChecksumWeights = { 3, 4, 5, 6, 7, 8, 9, 1, 2, 3 };
            int sum = 0; 
            int personalIdentifierLastDigit = digits.ElementAt(^1);

            for (int i = 0; i < personalIdentifierCode.Length - 1; i++)
            {
                sum += digits[i] * firstChecksumWeights[i];
            }
            int computedControlDigit = sum % 11;
            if (computedControlDigit == 10)
            {
                sum = 0;
                for (int i = 0; i < personalIdentifierCode.Length - 1; i++)
                {
                    sum += digits[i] * secondChecksumWeights[i];
                  
                }
                computedControlDigit = sum % 11;

                if (computedControlDigit == 10)
                {
                    computedControlDigit = 0;
                }
            }

           bool isControlDigitValid = computedControlDigit == personalIdentifierLastDigit;
            if (isControlDigitValid == false)
            {
                return EstonianPersonalCodeValidationResult.Failure(EstonianPersonalCodeValidationError.InvalidCheckDigit);
            }

            return EstonianPersonalCodeValidationResult.Success(dateOfBirth: parsedDateOfBirth, encodedSex: encodedSexValue);

            } 
         


            } 
    }

