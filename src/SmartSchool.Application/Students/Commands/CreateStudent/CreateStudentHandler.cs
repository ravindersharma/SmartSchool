using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Application.Students.Interfaces;

namespace SmartSchool.Application.Students.Commands.CreateStudent;

public class CreateStudentHandler : IRequestHandler<CreateStudentCommand, Result<StudentDto>>
{
    private readonly IStudentService _service;

    public CreateStudentHandler(IStudentService service) => _service = service;

    public Task<Result<StudentDto>> Handle(CreateStudentCommand request, CancellationToken ct) =>
       _service.CreateAsync(request.Dto, ct);

}
