using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;

namespace SmartSchool.Application.Students.Commands.CreateStudent;

public record CreateStudentCommand(string FullName, DateTime DOB, int Grade) : IRequest<Result<StudentDto>>;

