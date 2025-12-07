using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;

namespace SmartSchool.Application.Students.Commands.SoftDeleteStudent;


public record SoftDeleteStudentCommand(Guid Id) : IRequest<Result<StudentDto>>;
