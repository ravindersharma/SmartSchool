using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;

namespace SmartSchool.Application.Students.Queries.GetStudentByUserId;

public record GetStudentByUserIdQuery(Guid UserId) : IRequest<Result<StudentDto>>;
