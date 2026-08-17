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

        public PersonalIdentifierCodeValidatorResult Validate()
        {
         
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
