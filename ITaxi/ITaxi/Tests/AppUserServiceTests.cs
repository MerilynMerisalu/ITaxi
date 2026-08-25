using App.BLL.Services;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.Mappers;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public class AppUserServiceTests
    {
        public static TheoryData<DateOnly, DateOnly> Dates => new()
        {
            {new DateOnly(1993, 08, 14) , new DateOnly(1993, 08, 14)},

        };

        
        public static TheoryData<DateOnly, DateOnly> DatesDoesNotMatch => new()
        {
            {new DateOnly(1993, 08, 16) , new DateOnly(1993, 08, 14)},

        };
        private readonly Mock<IAppUserRepository> _appUserRepositoryMock;
        private readonly Mock<IMapper<App.BLL.DTO.Identity.AppUser, App.DAL.DTO.Identity.AppUser>> _appUserMapperMock;
        private readonly AppUserService _appUserService;
        public AppUserServiceTests()
        {
            _appUserRepositoryMock = new Mock<IAppUserRepository>();
            _appUserMapperMock = new Mock<IMapper<App.BLL.DTO.Identity.AppUser, App.DAL.DTO.Identity.AppUser>>();
            _appUserService = new AppUserService( _appUserRepositoryMock.Object, _appUserMapperMock.Object );
        }

        [Theory]
        [MemberData(nameof(Dates))]
        public void ValidateUsersChosenDateOfBirth_WhenDatesMatch_ReturnsTrue(DateOnly personalIdentifierDateOfBirth,
           DateOnly selectedDate)
        {
            
            // Act

            var result = _appUserService.ValidateUsersChosenDateOfBirth(personalIdentifierDateOfBirth,
                selectedDate);

            // Assert

            Assert.True(result);

        }


        [Theory]
        [MemberData(nameof(DatesDoesNotMatch))]
        public void ValidateUsersChosenDateOfBirth_WhenDatesDoNotMatch_ReturnsFalse(DateOnly personalIdentifierDateOfBirth,
           DateOnly selectedDate)
        {

            // Act

            var result = _appUserService.ValidateUsersChosenDateOfBirth(personalIdentifierDateOfBirth,
                selectedDate);

            // Assert

            Assert.False(result);

        }
    }

}
    

