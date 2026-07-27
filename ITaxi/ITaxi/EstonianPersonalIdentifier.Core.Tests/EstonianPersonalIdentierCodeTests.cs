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
            Assert.False(validationResult.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.ContainsNonDigits, validationResult.Error);
        }


    }
}
