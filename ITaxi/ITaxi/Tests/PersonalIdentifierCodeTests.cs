using App.BLL.DTO.Identity;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.Identity;
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

            _appUserServiceMock.Verify(service => service.ValidateUsersChosenDateOfBirth(personalIdentifierDateOfBirth, selectedDate), Times.Once());

        }

        [Theory]
        [MemberData(nameof(Data))]
        public void ValidateChosenDateOfBirth_WhenTheSelectedDateOfBirthDoesNotMatchPersonalIdentifierDateOfBirth_ReturnsFalse(DateOnly selectedDate, DateOnly personalIdentifierDateOfBirth)
        {
            // Arrange

            _appUserServiceMock.Setup(s => s.ValidateUsersChosenDateOfBirth(personalIdentifierDateOfBirth, selectedDate)).Returns(false);

            // Act

            var result = _validator.ValidateChosenDateOfBirth(personalIdentifierDateOfBirth, selectedDate);

            // Assert

            Assert.False(result);
        }
        
        [Theory]
        [MemberData(nameof(Dates))]
        public void ValidateUsersChosenDateOfBirth_WhenDatesMatch_ReturnsTrue(DateOnly personalIdentifierDateOfBirth, DateOnly selectedDate)
        {
            // Arrange

            var appUserRepositoryMock = new Mock<IAppUserRepository>();
            var mapperMock = new Mock<IMapper<App.BLL.DTO.Identity.AppUser, App.DAL.DTO.Identity.AppUser>>();
            var appUserService = new AppUserService(appUserRepositoryMock.Object, mapperMock.Object);

            // Act

            var result = appUserService.ValidateUsersChosenDateOfBirth(personalIdentifierDateOfBirth, 
                selectedDate);

            // Assert

            Assert.True(result);



        }
    }
}