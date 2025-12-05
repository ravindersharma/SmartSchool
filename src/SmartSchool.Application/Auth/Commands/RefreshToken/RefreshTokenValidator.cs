using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchool.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");

            RuleFor(x => x.IpAddress)
                .NotEmpty().WithMessage("IP address is required.");
        }
    }
}
