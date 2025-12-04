using FluentResults;
using MediatR;
using SmartSchool.Application.Auth.Dtos;

namespace SmartSchool.Application.Auth.Commands.ResetPassword
{
    public record ResetPasswordCommand(ResetPasswordRequestDto Dto,string Origin) : IRequest<Result>;
}
