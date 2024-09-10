using FluentValidation;
using FluentValidation.Results;

namespace Paynet.Challenge.DataContract.V1.Auth
{
    public class ForgotPasswordContract
    {
        public string Email { get; set; }

        public string NewPassword { get; set; }

        public bool Validate(out ValidationResult validationResult)
        {
            var validator = new InlineValidator<ForgotPasswordContract>();
            validator.RuleFor(x => x.Email)
               .NotEmpty()
               .EmailAddress();

            validator.RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(8);

            validationResult = validator.Validate(this);
            return validationResult.IsValid;
        }
    }
}