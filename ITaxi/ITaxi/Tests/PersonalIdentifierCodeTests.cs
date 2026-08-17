using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
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

       

        [Theory]
        [MemberData(nameof(Dates))]
        public void ValidateChosenDateOfBirth_WhenTheSelectedDateOfBirthMatchesPersonalIdentifierDateOfBirth_ReturnsTrue(DateOnly selectedDate, DateOnly personalIdentifierDateOfBirth)
        {
            // Arrange

            _appUserServiceMock.Setup(service => service.ValidateUsersChosenDateOfBirth(personalIdentifierDateOfBirth, selectedDate)).Returns(true);

            // Act

            var result = _validator.ValidateChosenDateOfBirth(personalIdentifierDateOfBirth, selectedDate);
            // Assert
            Assert.True(result);

            _appUserServiceMock.Verify(service => service.ValidateUsersChosenDateOfBirth(personalIdentifierDateOfBirth, selectedDate), Times.Once());
            
        }
    }
}