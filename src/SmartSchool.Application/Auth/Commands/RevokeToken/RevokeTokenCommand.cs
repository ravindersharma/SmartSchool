using FluentResults;
using MediatR;

namespace SmartSchool.Application.Auth.Commands.RevokeToken
{
    public record RevokeTokenCommand(string Token,string IpAddress) : IRequest<Result>;
}
