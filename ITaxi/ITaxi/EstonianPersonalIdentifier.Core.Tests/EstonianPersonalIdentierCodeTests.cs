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
        
        public void IsFirstDigitInvalid_WhenFirstDigitIsInvalid_ReturnsInvalidFirstDigitError(string personalIdentifierCode)
        {
            // Act
            var validationResult = EstonianPersonalCodeValidator.Validate(personalIdentifierCode);

            // Assert
            Assert.False(validationResult.IsValid);
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
    }
}

        