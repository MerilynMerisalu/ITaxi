using EstonianPersonalCode.Core;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EstonianPersonalIdentifier.Core.Tests
{
    public class EstonianPersonalIdentierCodeTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        
        public void Validate_WhenPersonalIdentifierCodeIsEmptyOrWhitespace_ReturnsEmptyError(string? personalIdentifierCode)
        {
            // Act
            var validationResult = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(EstonianPersonalCodeValidationError.Empty, validationResult.Error);

        }

        [Fact]
        public void Validate_WhenIsPersonalIdentifierCodeLengthInvalid_ReturnsInvalidLengthError()
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
    }
}

        