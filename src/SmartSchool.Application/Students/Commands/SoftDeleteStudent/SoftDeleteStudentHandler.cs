using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Commands.SoftDeleteStudent;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Application.Students.Interfaces;

namespace SmartSchool.Application.Students.Commands.SoftDeleteStudentHandler;

public class SoftDeleteStudentHandler : IRequestHandler<SoftDeleteStudentCommand, Result<StudentDto>>
{
    private readonly IStudentService _service;

    public SoftDeleteStudentHandler(IStudentService service) => _service = service;

    public  Task<Result<StudentDto>> Handle(SoftDeleteStudentCommand request, CancellationToken ct)=> _service.SoftDeleteAsync(request.Id, ct); 
}
