using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;

namespace SmartSchool.Application.Students.Commands.CreateStudent;

public record CreateStudentCommand(string FirstName, string LastName, DateTime DOB, int Grade) : IRequest<Result<StudentDto>>;

