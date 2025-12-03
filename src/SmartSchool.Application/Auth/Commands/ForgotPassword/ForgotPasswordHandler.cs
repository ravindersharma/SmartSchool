using FluentResults;
using MediatR;
using SmartSchool.Application.Auth.Interfaces;

namespace SmartSchool.Application.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordHandler:IRequestHandler<ForgotPasswordCommand,Result>
    {
        private readonly IAuthService _auth;    
        public ForgotPasswordHandler(IAuthService auth)=>_auth=auth;

        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken ct)=> await _auth.ForgotPasswordAsync(request.Email,request.Origin, ct);
    }
}
