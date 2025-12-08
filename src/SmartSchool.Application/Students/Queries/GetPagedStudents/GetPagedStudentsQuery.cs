using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Shared;

namespace SmartSchool.Application.Students.Queries.GetPagedStudents;

public record GetPagedStudentsQuery(int Page, int PageSize) : IRequest<Result<PagedResult<StudentDto>>>;
