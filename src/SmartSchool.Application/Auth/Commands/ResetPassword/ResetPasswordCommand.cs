using FluentResults;
using MediatR;
using SmartSchool.Application.Auth.Dtos;

namespace SmartSchool.Application.Auth.Commands.ResetPassword
{
    public record ResetPasswordCommand(ResetPasswordRequestDto Request) : IRequest<Result>;
}
