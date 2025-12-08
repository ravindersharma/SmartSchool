using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Interfaces;

namespace SmartSchool.Application.Students.Commands.DeleteStudentCommand;

public class DeleteStudentHandler : IRequestHandler<DeleteStudentCommand, Result>
{
    private readonly IStudentService _service;

    public DeleteStudentHandler(IStudentService service) => _service = service;

    public  Task<Result> Handle(DeleteStudentCommand request, CancellationToken ct)=> _service.DeleteAsync(request.Id, ct); 
}
