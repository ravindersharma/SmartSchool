using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Application.Students.Interfaces;

namespace SmartSchool.Application.Students.Commands.UpdateStudent;

public class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand, Result<StudentDto>>
{

    private readonly IStudentService _service;

    public UpdateStudentHandler(IStudentService service)=>_service = service;

    public  Task<Result<StudentDto>> Handle(UpdateStudentCommand request, CancellationToken ct)=> _service.UpdateAsync(request.Id, request.Dto, ct);
}
