using FluentResults;
using MediatR;
using SmartSchool.Application.Auth.Dtos;
using SmartSchool.Application.Auth.Interfaces;

namespace SmartSchool.Application.Auth.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
    {
        private readonly IAuthService _auth;

        public LoginHandler(IAuthService auth) => _auth = auth;

        public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken ct) => await _auth.LoginAsync(request.Request, request.IpAddress, ct);
    }
}
