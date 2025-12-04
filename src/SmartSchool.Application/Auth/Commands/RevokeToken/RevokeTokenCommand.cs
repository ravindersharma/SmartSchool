using FluentResults;
using MediatR;

namespace SmartSchool.Application.Auth.Commands.RevokeToken
{
    public record RevokeTokenCommand(string RefreshToken, string IpAddress) : IRequest<Result>;
}
