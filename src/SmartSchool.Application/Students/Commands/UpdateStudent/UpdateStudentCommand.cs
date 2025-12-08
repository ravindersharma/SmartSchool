using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;

namespace SmartSchool.Application.Students.Commands.UpdateStudent;

public record UpdateStudentCommand(Guid Id, UpdateStudentDto Dto) : IRequest<Result<StudentDto>>;
