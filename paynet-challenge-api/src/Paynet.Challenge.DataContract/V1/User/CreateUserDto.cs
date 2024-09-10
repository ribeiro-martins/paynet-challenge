using FluentValidation;
using FluentValidation.Results;
using Paynet.Challenge.Entities.CommonTypes;

namespace Paynet.Challenge.DataContract.V1.User
{
    public class CreateUserDto : UserDto
    {
        public Address Address { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public bool Validate(out ValidationResult validationResult)
        {
            var validator = new InlineValidator<CreateUserDto>();
             validator.RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            validator.RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);

            validator.RuleFor(x => x.FullName)
                .NotEmpty();

            validator.RuleFor(x => x.Address.Number)
                .NotEmpty();

            validator.RuleFor(x => x.Address.Neighborhood)
                .NotEmpty();

            validator.RuleFor(x => x.Address.State)
                .NotEmpty();
                
            validator.RuleFor(x => x.Address.City)
                .NotEmpty();

            validator.RuleFor(x => x.Address.State)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(2);

            validator.RuleFor(x => x.Address.ZipCode)
                .NotEmpty()
                .Length(8);

            validationResult = validator.Validate(this);
            return validationResult.IsValid;
        }
    }
}