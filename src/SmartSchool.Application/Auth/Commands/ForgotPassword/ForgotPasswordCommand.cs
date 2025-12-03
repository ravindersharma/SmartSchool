using FluentResults;
using MediatR;

namespace SmartSchool.Application.Auth.Commands.ForgotPassword;

public record ForgotPasswordCommand(string Email, string Origin) : IRequest<Result>;
