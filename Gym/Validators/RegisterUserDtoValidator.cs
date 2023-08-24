using FluentValidation;
using Gym.Dto;

namespace Gym.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(x => x.FirstName).NotNull().Length(1, 255);
            RuleFor(x => x.MiddleName).NotNull().Length(1, 255);
            RuleFor(x => x.LastName).NotNull().Length(1, 255);

            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.Password).NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]")
                .Matches("[a-z]")
                .Matches("[0-9]")
                .Matches("[^a-zA-Z0-9]");
            RuleFor(x => x.DateofBirth).NotNull()
                .Must(BeValidDate);
            RuleFor(x => x.ConfirmPassword).NotNull()
                .Equal(x => x.Password);
        }

        private bool BeValidDate(DateTime date)
        {
            return (DateTime.Now.Year - date.Year) >= 18;
        }
    }
}