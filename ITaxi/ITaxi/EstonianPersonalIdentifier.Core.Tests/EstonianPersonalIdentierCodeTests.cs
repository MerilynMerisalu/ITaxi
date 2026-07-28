using EstonianPersonalCode.Core;

namespace EstonianPersonalIdentifier.Core.Tests
{
    public class EstonianPersonalIdentierCodeTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Validate_WhenInputIsNullOrWhiteSpace_ReturnsEmptyError(string? personalIdentierCode)
        {
           
            // Act
            var validationResult = EstonianPersonalCodeValidator.Validate(personalIdentierCode);
            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.Empty, validationResult.Error);

        }

        [Theory]
        [InlineData("393081509812", 12)]
        [InlineData("4930613092", 10)]
        public void Validate_WhenInputLengthIsInvalid_ReturnsInvaildLengthError(string personalIdentifierCode, int expectedInputLength)
        {
            // Arrange 
            var actualLength = personalIdentifierCode.Length;
            // Verify test data
            Assert.Equal(expectedInputLength, actualLength);
            // Act
            var validationResult = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);
            
            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.InvalidLength, validationResult.Error);
            
        }
        [Theory]
        [InlineData("4930814087a")]
        [InlineData(".6200914838")]
        public void Validate_WhenInputContainsNonDigits_ReturnsContainsNonDigitsError(string personalIdentifierCode)
        {
            // Arrange

            const int expectedInputLength = 11;
            var actualLength = personalIdentifierCode.Length;
            // Verify test data
            Assert.Equal(expectedInputLength, actualLength);

            // Act
            var validationResult = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);

            // Assert
            Assert.False(personalIdentifierCode.All(c => c >= '0' && c <= '9'));
            Assert.False(validationResult.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.ContainsNonDigits, validationResult.Error);
        }
        [Theory]
        [InlineData("08907120031")]
        [InlineData("98806141141")]
        public void Validate_WhenInputStartsWithInvalidDigit_ReturnsInvalidFirstDigitError(string personalIdentifierCode)
        {
            // Arrange
            const int expectedInputLength = 11;
            var actualLength = personalIdentifierCode.Length;
            int firstDigit = int.Parse(personalIdentifierCode.ElementAt(0).ToString());
           
            // Verify test data
            Assert.Equal(expectedInputLength, actualLength);
            Assert.True(firstDigit < 1 || firstDigit > 8);
            Assert.True(personalIdentifierCode.All(c => c >= '0' && c <= '9'));

            // Act
            var validationResult = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);
            Assert.False(validationResult.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.InvalidFirstDigit, validationResult.Error);
        }
        [Theory]
        [InlineData("38907120035", EncodedSex.Male)]
        [InlineData("68806141144", EncodedSex.Female)]
        public void Validate_WhenFirstDigitIsOddOrEven_ReturnsExpectedEncodedSex(string personalIdentifierCode, EncodedSex encodedSex)
        {
            // Arrange
            const int expectedInputLength = 11;
            var actualLength = personalIdentifierCode.Length;
            int firstDigit = int.Parse(personalIdentifierCode.ElementAt(0).ToString());
           

            // Verify test data
            Assert.Equal(expectedInputLength, actualLength);
            Assert.True(firstDigit >= 1 && firstDigit <= 8);
            Assert.True(personalIdentifierCode.All(c => c >= '0' && c <= '9'));
          
            // Act
            var validationResult = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);
            // Assert
            Assert.Equal(encodedSex, validationResult.EncodedSex);
            Assert.True(validationResult.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.None, validationResult.Error);

            
        }

    }
}
