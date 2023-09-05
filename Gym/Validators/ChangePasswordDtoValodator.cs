using FluentValidation;
using Gym.Dto;

namespace Gym.Validators
{
    public class ChangePasswordDtoValodator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValodator()
        {
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8)
                .Matches("[A-Z]")
                .Matches("[a-z]")
                .Matches("[0-9]")
                .Matches("[^a-zA-Z0-9]");
        }
    }
}