using EstonianPersonalCode.Core;


namespace EstonianPersonalIdentifier.Core.Tests
{
    public class EstonianPersonalIdentifierCodeTests
    {
        public static TheoryData<DateOnly> IsEarlierThanMinimumDateTestData => new()
        {
            {new DateOnly(1700, 08, 08)  },
            {new DateOnly(1799, 12, 31)  },


        };
        public static TheoryData<DateOnly> MinimumDateOrLaterTestData => new()
        {
            {new DateOnly(1800, 01,01)  },
            {new DateOnly(1800, 01, 02)  },
        };
        public static TheoryData<DateOnly, bool, bool> IsTodayOrInFutureTestData => new()
            {
                { DateOnly.FromDateTime(DateTime.Today).AddDays(-1), true,  false },
                { DateOnly.FromDateTime(DateTime.Today).AddDays(-1), false, false },

                { DateOnly.FromDateTime(DateTime.Today),             true,  false },
                { DateOnly.FromDateTime(DateTime.Today),             false, true  },

                { DateOnly.FromDateTime(DateTime.Today).AddDays(10), true,  true  },
                { DateOnly.FromDateTime(DateTime.Today).AddDays(10), false, true  }
            };

        public static TheoryData<DateOnly> IsDateInFutureTestData => new()
        {
            
            {DateOnly.FromDateTime(DateTime.Today).AddDays(10)   },
            {DateOnly.FromDateTime(DateTime.Today)},

        };
        public static TheoryData<string, string, DateOnly> ValidEncodedDateTestData => new()
        {
            {"52608072223", "20", new DateOnly(2026, 08, 07)  },
            {"10001012233", "18" ,new DateOnly(1800, 01, 01) },
            {"10001022234", "18", new DateOnly(1800, 01, 02) },
            {"62402291114", "20", new DateOnly(2024, 02, 29) }

        };
        public static TheoryData<int[]> ValidatedDigitsForFirstWeight => new()
        {

            {[4,9,3,0,8,1,4,0,8,7,8] }

        };


        public static TheoryData<int[]> ValidatedDigitsForSecondWeight => new()
        {

            {[3,0,0,0,1,0,1,0,1,8,7] }

        };

        public static TheoryData<int[]> ValidatedDigitsEndingZeroForSecondWeight => new()
        {

            {[3,0,0,0,1,0,1,0,0,6,0] }

        };

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]

        public void PersonalIdentifierCodeIsEmptyOrWhitespace_WhenPersonalIdentifierCodeIsEmptyOrWhitespace_ReturnsEmptyError(string? personalIdentifierCode)
        {
            // Act
            var validationResult = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.Empty, validationResult.Error);

        }

        [Fact]
        public void IsPersonalIdentifierCodeLengthInvalid_WhenIsPersonalIdentifierCodeLengthInvalid_ReturnsInvalidLengthError()
        {
            // Arrange
            const int EXPECTEDLENGTH = 11;
            string personalIdentifierCode = "393081408781";

            // Act
            var validateResult = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);

            // Assert
            Assert.NotEqual(EXPECTEDLENGTH, personalIdentifierCode.Length);
            Assert.False(validateResult.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.InvalidLength, validateResult.Error);
        }

        [Theory]
        [InlineData("393081 4087")]
        [InlineData(".3780613089")]
        [InlineData("a4920515221")]
        public void PersonalIdentifierCodeConsistOnlyDigits_WhenPersonalIdentifierCodeContainsNonDigits_ReturnsEmptyArray(string personalIdentifierCode)
        {
            // Act
            var validationResult = EstonianPersonalCodeValidator.DoesPersonalIdentifierCodeHaveNonDigits(personalIdentifierCode, out int[] digits);

            // Assert
            Assert.Empty(digits);

        }
        [Theory]
        [InlineData("393081 4087")]
        [InlineData(".3780613089")]
        [InlineData("a4920515221")]
        public void PersonalIdentifierCodeConsistOnlyDigits_WhenDigitsArrayEmpty_ThrowsArgumentOutOfRangeExpection(string personalIdentifierCode)
        {
            // Act
            var validationResult = EstonianPersonalCodeValidator.DoesPersonalIdentifierCodeHaveNonDigits(personalIdentifierCode, out int[] digits);

            Assert.Empty(digits);
        }
        [Theory]
        [InlineData("393081 4087")]
        [InlineData(".3780613089")]
        [InlineData("a4920515221")]
        public void PersonalIdentifierCodeConsistOnlyDigits_WhenDigitsArrayEmpty_ReturnsInvalidLengthError(string personalIdentifierCode)
        {
            // Act
            var validationResult = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.ContainsNonDigits, validationResult.Error);


        }

        [Theory]
        [InlineData(9)]
        [InlineData(0)]
        //   [InlineData("19304218980")]
        //[InlineData("89811152221")]

        public void IsFirstDigitInvalid_WhenFirstDigitIsInvalid_ReturnsTrue(int firstDigit)
        {
            // Act
            var validationResult = EstonianPersonalCodeValidator.IsFirstDigitInvalid(firstDigit);

            // Assert
            Assert.True(validationResult);

        }
        [Theory]
        [InlineData("99304218980")]
        [InlineData("09811152221")]
        public void IsFirstDigitInvalid_WhenFirstDigitIsInvalid_ReturnsFirstDigitInvalidError(string personalIdentifierCode)
        {
            // Act
            var validationResult = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);

            // Assert
            Assert.Equal(EstonianPersonalCodeValidationError.InvalidFirstDigit, validationResult.Error);

        }
        [Theory]
        [InlineData(1, EncodedSex.Male)]
        [InlineData(2, EncodedSex.Female)]
        [InlineData(3, EncodedSex.Male)]
        [InlineData(4, EncodedSex.Female)]
        [InlineData(5, EncodedSex.Male)]
        [InlineData(6, EncodedSex.Female)]

        public void GetEncodedSex_WhenPersonalIdentifierCodeIsValid_ReturnsValidEncodedSex(int firstDigit, EncodedSex expectedEncodedSex)
        {
            // Act
            var validationResult = EstonianPersonalCodeValidator.GetEncodedSex(firstDigit);
            // Assert
            Assert.Equal(expectedEncodedSex, validationResult);

        }

        [Theory]
        [InlineData(1, "18")]
        [InlineData(2, "18")]
        [InlineData(3, "19")]
        [InlineData(4, "19")]
        [InlineData(5, "20")]
        [InlineData(6, "20")]
        [InlineData(7, "21")]
        [InlineData(8, "21")]
        public static void GetYearPrefix_WhenPersonalIdentifierCodeIsValid_ReturnsExpectedYearPrefix(int firstDigit, string expectedYearBase)
        {
            // Act
            var validationResult = EstonianPersonalCodeValidator.GetYearPrefix(firstDigit);
            // Assert
            Assert.Equal(expectedYearBase, validationResult);

        }

        [Theory]
        [MemberData(nameof(MinimumDateOrLaterTestData))]
        public void IsDateLessThanTheMinimumDate_WhenDateEqualsOrIsAfterMinimumDate_ReturnsFalse(DateOnly minimumValidDate)
        {
            

            // Act
            var validateResult = EstonianPersonalCodeValidator.IsDateLessThanTheMinimumDate(minimumValidDate);

            // Assert
            Assert.False(validateResult);
            
        }

        [Theory]
        [MemberData(nameof(ValidEncodedDateTestData))]
        public static void IsDateValid_WhenCodeContainsValidDate_ReturnsTrueAndParsedDate(string personalIdentifierCode,
            string yearPrefix, DateOnly expectedDate)
        {

            // Act

            var isValid = EstonianPersonalCodeValidator.IsDateValid(personalIdentifierCode, yearPrefix,
                out DateOnly validatedDate);
            // Assert
            Assert.True(isValid);
            Assert.Equal(expectedDate, validatedDate);

        }

        
        [Theory]
        [MemberData(nameof(IsTodayOrInFutureTestData))]
        public static void IsDateInFuture_WhenTodayAllowanceVaries_ReturnsExpectedResult(DateOnly date, bool isTodaysDateAllowed, 
            bool expectedIsInvalid )
        {
            // Act
            var isInvalid = EstonianPersonalCodeValidator.IsDateInFuture(date, isTodaysDateAllowed);

            // Assert
            Assert.Equal(expectedIsInvalid, isInvalid);
            
        }

      

        [Theory]
        [MemberData(nameof(IsDateInFutureTestData))]
        public static void IsDateTodayOrInTheFuture_WhenTodaysDateIsNotAllowed_ReturnsTrue(DateOnly date)
        {
            // Act
            var isInvalid = EstonianPersonalCodeValidator.IsDateInFuture(date);

            // Assert
            Assert.True(isInvalid);
        }

        [Theory]
        [MemberData(nameof(IsEarlierThanMinimumDateTestData))]
        public static void IsDateLessThanTheMinimumDate_WhenTheDateIsLessThanTheMinimumDate_ReturnsTrue(DateOnly date)
        {
          
            // Act
            var validationResult = EstonianPersonalCodeValidator.IsDateLessThanTheMinimumDate(date);
            // Assert
            Assert.True(validationResult);
        }


        [Theory]
        [InlineData("52613062231")]
        [InlineData("52302290123")]
        public static void Validate_WhenEncodedDateIsInvalid_ReturnsInvalidEncodedDateError(string personalIdentifierCode)
        {
            // Act
            var result = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.InvalidEncodedDate, result.Error);

        }

        [Theory]
        [InlineData("72612062231")]
        
        public static void Validate_WhenEncodedDateIsInFuture_ReturnsInvalidDateError(string personalIdentifierCode)
        {
            // Act
            var result = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.InvalidDate, result.Error);

        }

        [Theory]
        [MemberData(nameof(ValidatedDigitsForFirstWeight))]
        public static void ComputeChecksumDigit_WhenPersonalIdentifierCodeIsValidUsingFirstWeights_ReturnsCheckDigit(int[] digits)
        {
            
            // Act

            int result = EstonianPersonalCodeValidator.ComputeCheckDigit(digits, out bool isCalculatedUsingFirstWeights);

            // Assert
            Assert.InRange(result, 0, 9);
            Assert.True(isCalculatedUsingFirstWeights);
        }

        [Theory]
        [MemberData(nameof(ValidatedDigitsForSecondWeight))]
        public static void ComputeChecksumDigit_WhenPersonalIdentifierCodeIsValidUsingSecondWeights_ReturnsCheckDigit(int[] digits)
        {

            // Act

            int result = EstonianPersonalCodeValidator.ComputeCheckDigit(digits, out bool isCalculatedUsingFirstWeights);

            // Assert
            Assert.InRange(result, 0, 9);
            Assert.False(isCalculatedUsingFirstWeights);
        }

        [Theory]
        [MemberData(nameof(ValidatedDigitsEndingZeroForSecondWeight))]
        public static void ComputeChecksumDigit_WhenPersonalIdentifierCodeEndsZeroIsValidUsingSecondWeights_ReturnsCheckDigit(int[] digits)
        {

            // Act

            int result = EstonianPersonalCodeValidator.ComputeCheckDigit(digits, out bool isCalculatedUsingFirstWeights);

            // Assert
            Assert.Equal(0, result);
            Assert.False(isCalculatedUsingFirstWeights);
        }

        [Theory]
        [InlineData(8, 8)]
        public static void IsNotEqualToLastDigitOfPersonalIdentifierCode_WhenTheLastDigitEqualWithControlDigit_ReturnsFalse(int lastDigit, int checkDigit)
        {
            // Act
            bool result = EstonianPersonalCodeValidator.IsNotEqualToLastDigitOfPersonalIdentifierCode(lastDigit, checkDigit);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("49308140875")]
        public static void IsNotEqualToLastDigitOfPersonalIdentifierCode_WhenTheLastDigitEqualWithControlDigit_ReturnsInvalidCheckDigitError(string personalIdentifierCode)
        {
            // Act
            var result = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.InvalidCheckDigit, result.Error);
        }

        [Theory]
        [InlineData("49308140878")]
        public static void Validate_WhenThePersonalIdentifierCodeIsValid_ReturnsValidPersonalIdentifierCode(string personalIdentifierCode)
        {
            // Act
            var result = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.None, result.Error);
        }
    }

    
}

        