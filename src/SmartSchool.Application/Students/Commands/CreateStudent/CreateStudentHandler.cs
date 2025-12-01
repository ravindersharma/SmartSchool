using FluentResults;
using MapsterMapper;
using MediatR;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Application.Students.Interfaces;
using SmartSchool.Domain.Entities;

namespace SmartSchool.Application.Students.Commands.CreateStudent
{
    public class CreateStudentHandler : IRequestHandler<CreateStudentCommand, Result<StudentDto>>
    {
        private readonly IStudentRepository _repo;
        private readonly IMapper _mapper;

        public CreateStudentHandler(IStudentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<StudentDto>> Handle(CreateStudentCommand request, CancellationToken ct)
        {
            var entity = new Student
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName.Trim(),  
                DOB =request.DOB,
                Grade = request.Grade,
                CreatedAt = DateTime.UtcNow
            };


            var saved=await _repo.AddAsync(entity,ct);

            var dto=_mapper.Map<StudentDto>(saved);
            return Result.Ok(dto);
        }
    }
}
