using FluentValidation;

namespace SmartSchool.Application.Auth.Commands.RevokeToken
{
    public class RevokeTokenValidator : AbstractValidator<RevokeTokenCommand>
    {
        public RevokeTokenValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");

            RuleFor(x => x.IpAddress)
                .NotEmpty().WithMessage("IP address is required.");
        }
    }
}
