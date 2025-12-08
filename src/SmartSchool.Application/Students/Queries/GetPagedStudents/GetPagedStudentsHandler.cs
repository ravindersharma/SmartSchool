using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Application.Students.Interfaces;
using SmartSchool.Shared;

namespace SmartSchool.Application.Students.Queries.GetPagedStudents;

public class GetPagedStudentsHandler : IRequestHandler<GetPagedStudentsQuery, Result<PagedResult<StudentDto>>>
{
    private readonly IStudentService _service;
    public GetPagedStudentsHandler(IStudentService service) => _service = service;
    public  Task<Result<PagedResult<StudentDto>>> Handle(GetPagedStudentsQuery request, CancellationToken ct) => _service.GetPagedAsync(request.Page, request.PageSize, ct);

}
