using FluentValidation;
using SmartSchool.Application.Auth.Commands.RegisterUser;

namespace SmartSchool.Application.Auth.Commands.Register
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Dto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Dto.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.");

            RuleFor(x => x.Dto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .Matches("[A-Z]").WithMessage("Must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Must contain at least one special character.");

            RuleFor(x => x.Origin)
                .NotEmpty().WithMessage("Origin header is required.");
        }
    }
}
