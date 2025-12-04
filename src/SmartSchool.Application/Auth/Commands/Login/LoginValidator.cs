using FluentValidation;

namespace SmartSchool.Application.Auth.Commands.Login
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Dto.Email)
                .NotEmpty().WithMessage("Email is required.");

            RuleFor(x => x.Dto.Password)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(x => x.IpAddress)
                .NotEmpty().WithMessage("IP address is required.");
        }
    }
}
