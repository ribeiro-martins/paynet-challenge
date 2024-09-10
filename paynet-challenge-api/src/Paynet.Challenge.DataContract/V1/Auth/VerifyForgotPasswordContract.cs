using FluentValidation;
using FluentValidation.Results;

namespace Paynet.Challenge.DataContract.V1.Auth
{
    public class VerifyForgotPasswordContract
    {
        public string Email { get; set; }

        public string SecretCode { get; set; }

        public bool Validate(out ValidationResult validationResult)
        {
            var validator = new InlineValidator<VerifyForgotPasswordContract>();
            validator.RuleFor(x => x.Email)
               .NotEmpty()
               .EmailAddress();

            validator.RuleFor(x => x.SecretCode)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(6);

            validationResult = validator.Validate(this);
            return validationResult.IsValid;
        }
    }
}