using FluentResults;
using MediatR;
using SmartSchool.Application.Auth.Dtos;
using SmartSchool.Application.Auth.Interfaces;

namespace SmartSchool.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResponseDto>>
    {
        private readonly IAuthService _auth;
        public RefreshTokenHandler(IAuthService auth) => _auth = auth;

        public async Task<Result<AuthResponseDto>> Handle(RefreshTokenCommand request, CancellationToken ct) => await _auth.RefreshTokenAsync(request.RefreshToken, request.IpAddress, ct);
    }
}
