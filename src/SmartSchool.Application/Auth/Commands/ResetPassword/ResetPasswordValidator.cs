using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchool.Application.Auth.Commands.ResetPassword
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Dto.Token)
                .NotEmpty().WithMessage("Reset token is required.");

            RuleFor(x => x.Dto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .Matches("[A-Z]").WithMessage("Must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Must contain at least one special character.");

            RuleFor(x => x.Dto.ConfirmPassword)
                .Equal(x => x.Dto.Password)
                .WithMessage("Password and Confirm Password must match.");

            RuleFor(x => x.Origin)
                .NotEmpty().WithMessage("Origin header is required.");
        }
    }
}
