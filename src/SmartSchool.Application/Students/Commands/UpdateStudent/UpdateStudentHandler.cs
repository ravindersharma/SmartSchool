using FluentResults;
using MapsterMapper;
using MediatR;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Application.Students.Interfaces;

namespace SmartSchool.Application.Students.Commands.UpdateStudent
{
    public class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand, Result<StudentDto>>
    {

        private readonly IStudentRepository _repo;
        private readonly IMapper _mapper;

        public UpdateStudentHandler(IStudentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<StudentDto>> Handle(UpdateStudentCommand request, CancellationToken ct)
        {
            var existing = await _repo.GetByIdAsync(request.Id, ct);

            if (existing == null) return Result.Fail("Student not found");

            existing.FullName = request.FullName;
            existing.Grade = request.Grade;
            existing.DOB = request.DOB;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(existing, ct);
            return Result.Ok(_mapper.Map<StudentDto>(updated));

        }
    }
}
