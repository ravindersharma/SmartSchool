using FluentResults;
using MediatR;
using SmartSchool.Application.Auth.Dtos;

namespace SmartSchool.Application.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(string Token, string IpAddress) : IRequest<Result<AuthResponseDto>>;
}
