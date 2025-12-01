using FluentResults;
using MediatR;
using SmartSchool.Application.Auth.Dtos;


namespace SmartSchool.Application.Auth.Commands.Login
{
    public record LoginCommand(LoginRequestDto Request, string IpAddress) : IRequest<Result<AuthResponseDto>>;
}
