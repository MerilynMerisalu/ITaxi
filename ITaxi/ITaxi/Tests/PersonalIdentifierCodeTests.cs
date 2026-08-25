using App.BLL.DTO.Identity;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.Identity;
using App.Enum.Enum;
using Base.Contracts.Mappers;
using Moq;
using PersonalIdentifierCode;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class PersonalIdentifierCodeTests
    {
        private readonly Mock<IAppUserService> _appUserServiceMock;
        private readonly PersonalIdentifierCodeValidator _validator;

        public PersonalIdentifierCodeTests()
        {

            _appUserServiceMock = new Mock<IAppUserService>();

            var appBLLMock = new Mock<IAppBLL>();

            appBLLMock
                .SetupGet(appBLL => appBLL.AppUsers)
                .Returns(_appUserServiceMock.Object);
            _validator = new PersonalIdentifierCodeValidator(appBLLMock.Object);

        }




        public static TheoryData<DateOnly, DateOnly> Dates => new()
        {
            {new DateOnly(1993, 08, 14) , new DateOnly(1993, 08, 14)},

        };


        public static TheoryData<DateOnly, DateOnly> Data => new()
        {
            {new DateOnly(1993, 08, 14) , new DateOnly(1993, 08, 16)},

        };
        public static TheoryData<int, Gender> Genders => new()
        {
            {2, Gender.Female },

        };
        [Theory]
        [MemberData(nameof(Dates))]
        public void ValidateChosenDateOfBirth_WhenTheSelectedDateOfBirthMatchesPersonalIdentifierDateOfBirth_ReturnsTrue(DateOnly personalIdentifierDateOfBirth, DateOnly selectedDate)
        {
            // Arrange

            _appUserServiceMock.Setup(service => service.ValidateUsersChosenDateOfBirth(personalIdentifierDateOfBirth, selectedDate)).Returns(true);

            // Act

            var result = _validator.ValidateChosenDateOfBirth(personalIdentifierDateOfBirth, selectedDate);
            // Assert
            Assert.True(result);

            _appUserServiceMock.Verify(service => service.ValidateUsersChosenDateOfBirth(personalIdentifierDateOfBirth, selectedDate),
                Times.Once());

        }

        [Theory]
        [MemberData(nameof(Data))]
        public void ValidateChosenDateOfBirth_WhenTheSelectedDateOfBirthDoesNotMatchPersonalIdentifierDateOfBirth_ReturnsFalse
             (DateOnly personalIdentifierDateOfBirth, DateOnly selectedDate)
        {
            // Arrange

            _appUserServiceMock.Setup(s => s.ValidateUsersChosenDateOfBirth(personalIdentifierDateOfBirth, selectedDate));

            // Act

            var result = _validator.ValidateChosenDateOfBirth(personalIdentifierDateOfBirth, selectedDate);

            // Assert

            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void ValidateChosenDateOfBirth_WhenTheSelectedDateOfBirthDoesNotMatchPersonalIdentifierDateOfBirth_ReturnsSelectedDateOfBirthDoesNotMatchError
             (DateOnly personalIdentifierDateOfBirth, DateOnly selectedDate)
        {
            // Arrange

            _appUserServiceMock.Setup(s => s.ValidateUsersChosenDateOfBirth(personalIdentifierDateOfBirth, selectedDate)).Returns(false);

            // Act

            var result = _validator.Validate(personalIdentifierDateOfBirth, selectedDate);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(PersonalIdentifierCodeValidatorError.SelectedDateOfBirthDoesNotMatch, result.Error);
        }



        [Theory]
        [MemberData(nameof(Genders))]
        public void ValidateChosenGender_WhenTheSelectedGenderMatchesPersonalIdentifierCodeGender_ReturnsTrue
             (int personalIdentifierGender, Gender selectedGender)
        
            {
                // Arrange

                _appUserServiceMock.Setup(s => s.ValidateUsersGender(chosenGender: selectedGender, genderFromPersonalIdentifierCode: personalIdentifierGender)).Returns(true);

                // Act

                var result = _validator.ValidateChosenGender(personalIdentifierCodeGender: personalIdentifierGender, selectedGender: selectedGender);

                // Assert
                Assert.True(result);
                _appUserServiceMock.Verify(
                    s => s.ValidateUsersGender(
                    selectedGender,
                    personalIdentifierGender),
                    Times.Once());
        }

        
    }
}