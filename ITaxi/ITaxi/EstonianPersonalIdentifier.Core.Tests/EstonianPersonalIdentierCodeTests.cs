using EstonianPersonalCode.Core;


namespace EstonianPersonalIdentifier.Core.Tests
{
    public class EstonianPersonalIdentierCodeTests
    {
        
        public static TheoryData<DateOnly> IsEarlierThanMinimumDateTestData => new()
        {
            {new DateOnly(1700, 08, 08)  },
            {new DateOnly(1799, 12, 31)  },
            

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
           {DateOnly.FromDateTime(DateTime.Today)  },

        };
        public static TheoryData<string, string, DateOnly> TestData => new()
        {
            {"52608072223", "20", new DateOnly(2026, 08, 07)  },
            {"10001012233", "18" ,new DateOnly(1800, 01, 01) }
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
        public void PersonalIdentierCodeConsistOnlyDigits_WhenPersonalIdentifierCodeContainsNonDigits_ReturnsContainsNonDigitsError(string personalIdentifierCode)
        {
            // Act
            var validationResult = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.ContainsNonDigits, validationResult.Error);

        }

        [Theory]
        [InlineData("99304218980")]
        [InlineData("09811152221")]
        //   [InlineData("19304218980")]
        //[InlineData("89811152221")]

        public void IsFirstDigitInvalid_WhenFirstDigitIsInvalid_ReturnsTrue(string personalIdentifierCode)
        {
            // Act
            var validationResult = EstonianPersonalCodeValidator.IsFirstDigitInvalid(personalIdentifierCode);

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
        [InlineData("19304218980", EncodedSex.Male)]
        [InlineData("29811152221", EncodedSex.Female)]
        [InlineData("39304218980", EncodedSex.Male)]
        [InlineData("49811152221", EncodedSex.Female)]
        [InlineData("59304218980", EncodedSex.Male)]
        [InlineData("69811152221", EncodedSex.Female)]

        public void GetEncodedSex_WhenPersonalIdentifierCodeIsValid_ReturnsValidEncodedSex(string personalIdentifierCode, EncodedSex expectedEncodedSex)
        {
            // Act
            var validationResult = EstonianPersonalCodeValidator.GetEncodedSex(personalIdentifierCode);
            // Assert
            Assert.Equal(expectedEncodedSex, validationResult);

        }

        [Theory]
        [InlineData("19304218980", "18")]
        [InlineData("29811152221", "18")]
        [InlineData("39304218980", "19")]
        [InlineData("49811152221", "19")]
        [InlineData("59304218980", "20")]
        [InlineData("69811152221", "20")]
        [InlineData("70611152221", "21")]
        [InlineData("80111152221", "21")]
        public static void GetYearBase_WhenPersonalIdentifierCodeIsValid_ReturnsValidYearBase(string personalIdentifierCode, string expectedYearBase)
        {
            // Act
            var validationResult = EstonianPersonalCodeValidator.GetYearBase(personalIdentifierCode);
            // Assert
            Assert.Equal(expectedYearBase, validationResult);

        }

        [Fact]
        public void IsDateLessThanTheMinimumDate_WhenDateEqualsMinimumDate_ReturnsFalse()
        {
            // Arrange
            var minimumValidDate = new DateOnly(1800, 01, 01);

            // Act
            var validateResult = EstonianPersonalCodeValidator.IsDateLessThanTheMinimumDate(minimumValidDate);

            // Assert
            Assert.False(validateResult);
            
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public static void IsDateValid_WhenCodeContainsValidDate_ReturnsTrueAndParsedDate(string personalIdentifierCode,
            string baseYear, DateOnly expectedDate)
        {

            // Act

            var isValid = EstonianPersonalCodeValidator.IsDateValid(personalIdentifierCode, baseYear,
                out DateOnly validatedDate);
            // Assert
            Assert.True(isValid);
            Assert.Equal(expectedDate, validatedDate);

        }

        
        [Theory]
        [MemberData(nameof(IsTodayOrInFutureTestData))]
        public static void IsDateInFuture_WhenTodayAllowanceVaries_ReturnsExpectedResult(DateOnly date, bool isTodaysDateAllowed, bool expectedIsInvalid )
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
        public static void IsDateGreaterOrEqualThanToday_WhenDateFormatIsInvalid_ReturnsInvalidDateFormatError(string personalIdentifierCode)
        {
            // Act
            var result = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.InvalidDateFormat, result.Error);
            
        }


    }
}

        