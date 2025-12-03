using FluentResults;
using MediatR;
using SmartSchool.Application.Auth.Interfaces;

namespace SmartSchool.Application.Auth.Commands.RevokeToken
{
    public class RevokeTokenHandler:IRequestHandler<RevokeTokenCommand,Result>
    {
        private readonly IAuthService _auth;
        public RevokeTokenHandler(IAuthService auth)=>_auth=auth;
        public async Task<Result> Handle(RevokeTokenCommand request, CancellationToken ct)=> await _auth.RevokeRefreshTokenAsync(request.Token, request.IpAddress, ct);
    }
}
