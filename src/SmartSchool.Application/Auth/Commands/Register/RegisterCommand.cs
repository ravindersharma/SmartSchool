using FluentResults;
using MediatR;
using SmartSchool.Application.Auth.Dtos;

namespace SmartSchool.Application.Auth.Commands.RegisterUser
{
    public record RegisterCommand(RegisterRequestDto Request, string Origin) : IRequest<Result<AuthResponseDto>>;
}
