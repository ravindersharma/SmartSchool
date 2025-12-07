using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;

namespace SmartSchool.Application.Students.Queries.GetStudentById;

public record GetStudentByIdQuery(Guid Id) : IRequest<Result<StudentDto>>;
