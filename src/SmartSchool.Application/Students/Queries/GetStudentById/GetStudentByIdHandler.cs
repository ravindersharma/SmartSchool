using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Application.Students.Interfaces;

namespace SmartSchool.Application.Students.Queries.GetStudentById;

public class GetStudentByIdHandler:IRequestHandler<GetStudentByIdQuery,Result<StudentDto>>
{
    private readonly IStudentService _service;

    public GetStudentByIdHandler(IStudentService service)=>_service = service;

    public Task<Result<StudentDto>> Handle(GetStudentByIdQuery request, CancellationToken ct) => _service.GetByIdAsync(request.Id,ct);
}
