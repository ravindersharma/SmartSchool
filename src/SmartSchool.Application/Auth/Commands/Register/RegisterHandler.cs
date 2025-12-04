using FluentResults;
using MediatR;
using SmartSchool.Application.Auth.Commands.RegisterUser;
using SmartSchool.Application.Auth.Dtos;
using SmartSchool.Application.Auth.Interfaces;

namespace SmartSchool.Application.Auth.Commands.Register
{
    public class RegisterHandler:IRequestHandler<RegisterCommand,Result<AuthResponseDto>>
    {
        private readonly IAuthService _auth;
        public RegisterHandler(IAuthService auth) => _auth = auth;

        public async Task<Result<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken ct) => await _auth.RegisterAsync(request.Dto, request.Origin, ct);
    }
}
