using App.BLL.Services;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using App.Enum.Enum;
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

        public static TheoryData<DateOnly, DateOnly> DatesDoNotMatch => new()
        {
            {new DateOnly(1993, 08, 16) , new DateOnly(1993, 08, 14)},

        };

        public static TheoryData<int, Gender> Genders => new()
        {
            {2, Gender.Female }, {3, Gender.Custom},
            {4, Gender.Female }, {5, Gender.Male},
        };

        public static TheoryData<int, Gender> GendersDoNotMatch => new()
        {
            {3, Gender.Female }, {2, Gender.Male }
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
        [MemberData(nameof(DatesDoNotMatch))]
        public void ValidateUsersChosenDateOfBirth_WhenDatesDoNotMatch_ReturnsFalse(DateOnly personalIdentifierDateOfBirth,
           DateOnly selectedDate)
        {

            // Act

            var result = _appUserService.ValidateUsersChosenDateOfBirth(personalIdentifierDateOfBirth,
                selectedDate);

            // Assert

            Assert.False(result);

        }

        [Theory]
        [MemberData(nameof(Genders))]
        public void ValidateUsersGender_WhenGendersMatch_ReturnsTrue(int personalIdentifierGender,
           Gender selectedGender)
        {

            // Act

            var result = _appUserService.ValidateUsersGender(genderFromPersonalIdentifierCode: personalIdentifierGender, chosenGender: selectedGender);

            // Assert

            Assert.True(result);

        }
    

        [Theory]
        [MemberData(nameof(GendersDoNotMatch))]
        public void ValidateUsersGender_WhenGendersDoNotMatch_ReturnsFalse(int personalIdentifierGender,
           Gender selectedGender)
        {

            // Act

            var result = _appUserService.ValidateUsersGender(genderFromPersonalIdentifierCode: personalIdentifierGender, chosenGender: selectedGender);

            // Assert

            Assert.False(result);

        }
    }
}
    

