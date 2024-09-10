using FluentValidation;
using FluentValidation.Results;

namespace Paynet.Challenge.DataContract.V1.Auth
{
    public class LoginContract
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool Validate(out ValidationResult validationResult)
        {
            var validator = new InlineValidator<LoginContract>();
            validator.RuleFor(x => x.Email)
               .NotEmpty()
               .EmailAddress();

            validator.RuleFor(x => x.Password)
                .NotEmpty();

            validationResult = validator.Validate(this);
            return validationResult.IsValid;
        }
    }
}