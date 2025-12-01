using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;

namespace SmartSchool.Application.Students.Queries.GetPagedStudents
{
    public record GetPagedStudentsQuery(int Page, int PageSize) : IRequest<Result<IEnumerable<StudentDto>>>;
}
