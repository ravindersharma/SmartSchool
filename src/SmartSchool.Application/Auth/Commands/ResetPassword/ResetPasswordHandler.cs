using FluentResults;
using MediatR;
using SmartSchool.Application.Auth.Interfaces;

namespace SmartSchool.Application.Auth.Commands.ResetPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly IAuthService _auth;
        public ResetPasswordHandler(IAuthService auth) => _auth = auth;
        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken ct) => await _auth.ResetPasswordAsync(request.Request, ct);
    }
}
