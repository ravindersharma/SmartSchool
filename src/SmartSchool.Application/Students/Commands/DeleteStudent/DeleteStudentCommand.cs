using FluentResults;
using MediatR;

namespace SmartSchool.Application.Students.Commands.DeleteStudentCommand
{
    public record DeleteStudentCommand(Guid Id) : IRequest<Result>;
}
