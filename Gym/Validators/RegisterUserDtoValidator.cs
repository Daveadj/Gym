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
            RuleFor(x => x.YearOfBirth).NotNull().LessThanOrEqualTo(DateTime.Now.Year - 18)
                .GreaterThanOrEqualTo(DateTime.Now.Year - 100);
            RuleFor(x => x.DayOfBirth).NotNull().LessThanOrEqualTo(31).GreaterThanOrEqualTo(1);
        }
    }
}