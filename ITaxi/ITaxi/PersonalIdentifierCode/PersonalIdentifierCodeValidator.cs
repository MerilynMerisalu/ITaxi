using App.BLL.Services;
using App.Contracts.BLL;
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
             new DateOnly(1993, 08, 14), new DateOnly(1993, 08, 14));
            return result;
        } 



        
    }
}
