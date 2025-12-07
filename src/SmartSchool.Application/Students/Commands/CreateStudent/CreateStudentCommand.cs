using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;

namespace SmartSchool.Application.Students.Commands.CreateStudent;

public record CreateStudentCommand(CreateStudentDto Dto) : IRequest<Result<StudentDto>>;

