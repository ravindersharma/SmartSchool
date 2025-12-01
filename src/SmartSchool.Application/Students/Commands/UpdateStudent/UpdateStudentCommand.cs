using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;

namespace SmartSchool.Application.Students.Commands.UpdateStudent
{
    public record UpdateStudentCommand(Guid Id,string FullName, int Grade, DateTime DOB ) : IRequest<Result<StudentDto>>;
}
