using FluentResults;
using MapsterMapper;
using MediatR;
using SmartSchool.Application.Students.Interfaces;

namespace SmartSchool.Application.Students.Commands.DeleteStudentCommand
{
    public class DeleteStudentHandler : IRequestHandler<DeleteStudentCommand, Result>
    {
        private readonly IStudentRepository _repo;
        private readonly IMapper _mapper;

        public DeleteStudentHandler(IStudentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result> Handle(DeleteStudentCommand request, CancellationToken ct)
        {
            var student = await _repo.GetByIdAsync(request.Id, ct);
            if (student == null) return Result.Fail("Student not found");
            await _repo.DeleteAsync(student, ct);
            return Result.Ok();
        }



    }
}
