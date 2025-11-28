using AutoMapper;
using FluentResults;
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
                FirstName = request.FirstName,  
                LastName = request.LastName,
                DOB =request.DOB,
                Grade = request.Grade,
            };


            await _repo.AddAsync(entity,ct);

            var dto=_mapper.Map<StudentDto>(entity);
            return Result.Ok(dto);
        }
    }
}
