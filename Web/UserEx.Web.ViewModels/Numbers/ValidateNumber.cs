namespace UserEx.Web.ViewModels.Numbers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ValidateNumber : ValidationAttribute
    {
        protected override ValidationResult IsValid(object didNumber, ValidationContext validationContext)
        {
            var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();

            try
            {
                var phoneNumber = phoneNumberUtil.Parse($"+{didNumber}", null);

                var phoneNumberIsValid = phoneNumberUtil.IsValidNumber(phoneNumber);

                if (phoneNumberIsValid)
                {
                    return ValidationResult.Success;
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(e.Message);
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
