using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Application.Students.Interfaces;

namespace SmartSchool.Application.Students.Queries.GetStudentByUserId;

public class GetStudentByUserIdHandler : IRequestHandler<GetStudentByUserIdQuery,Result<StudentDto>>
{
    private readonly IStudentService _service;

    public GetStudentByUserIdHandler(IStudentService service)=>_service = service;

    public Task<Result<StudentDto>> Handle(GetStudentByUserIdQuery request, CancellationToken ct) => _service.GetByUserIdAsync(request.UserId,ct);
}
