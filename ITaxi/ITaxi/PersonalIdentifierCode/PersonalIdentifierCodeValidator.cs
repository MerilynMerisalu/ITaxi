using App.BLL.Services;
using App.Contracts.BLL;
using App.Enum.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalIdentifierCode
{
    public class PersonalIdentifierCodeValidator
    {
        private readonly IAppBLL _appBLL;

        public PersonalIdentifierCodeValidator(IAppBLL appBLL)
        {
            _appBLL = appBLL;
        }

        public PersonalIdentifierCodeValidatorResult Validate(DateOnly personalIdentifierDateOfBirth, DateOnly selectedDateOfBirth)
        {
            var result = ValidateChosenDateOfBirth(personalIdentifierDateOfBirth: personalIdentifierDateOfBirth, selectedDateOfBirth: selectedDateOfBirth);
            if (!result)
            {
                var validatorResult = new PersonalIdentifierCodeValidatorResult
                {
                    IsValid = false,
                    Error = PersonalIdentifierCodeValidatorError.SelectedDateOfBirthDoesNotMatch
                };
                return validatorResult;
            }
            throw new NotImplementedException();

        }

        public bool ValidateChosenDateOfBirth(DateOnly personalIdentifierDateOfBirth, DateOnly selectedDateOfBirth) 
        {
            var result = _appBLL.AppUsers.ValidateUsersChosenDateOfBirth(
             chosenDateOfBirth: selectedDateOfBirth, dateOfBirthFromPersonalIdentifierCode: personalIdentifierDateOfBirth);
            return result;
        }
        public bool ValidateChosenGender(int personalIdentifierCodeGender, Gender selectedGender)
        {
           
            var result = _appBLL.AppUsers.ValidateUsersGender(genderFromPersonalIdentifierCode: personalIdentifierCodeGender, chosenGender: selectedGender);
            return result;
        }



    }
}
