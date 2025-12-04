using FluentResults;
using MediatR;
using SmartSchool.Application.Auth.Dtos;


namespace SmartSchool.Application.Auth.Commands.Login
{
    public record LoginCommand(LoginRequestDto Dto, string IpAddress) : IRequest<Result<AuthResponseDto>>;
}
