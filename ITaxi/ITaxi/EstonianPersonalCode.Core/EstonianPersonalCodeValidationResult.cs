
using System;

namespace EstonianPersonalCode.Core
{
   public sealed class EstonianPersonalCodeValidationResult
    {
        
        public bool IsValid { get; init; }
        public DateOnly? DateOfBirth { get; init; }
        public EncodedSex? EncodedSex { get; init; }

        public EstonianPersonalCodeValidationError Error { get; init; }
        private EstonianPersonalCodeValidationResult()
        {
            
        }

        public static EstonianPersonalCodeValidationResult Success(DateOnly dateOfBirth, 
            EncodedSex encodedSex) 
        {
         
            EstonianPersonalCodeValidationResult result = new EstonianPersonalCodeValidationResult()
            { IsValid = true,
              DateOfBirth = dateOfBirth,
              EncodedSex = encodedSex,
              Error = EstonianPersonalCodeValidationError.None
            };

            return result;
        }
        
        public static EstonianPersonalCodeValidationResult Failure(EstonianPersonalCodeValidationError error)
        {
            if (error == EstonianPersonalCodeValidationError.None)
            {
                throw new ArgumentException("Error cannot be None when creating a failure result.", nameof(error));
            }

            EstonianPersonalCodeValidationResult result = new EstonianPersonalCodeValidationResult() {
                IsValid = false,
                Error = error,
                EncodedSex = null,
                DateOfBirth = null,
            };
            return result;
        }


    }

    
}
