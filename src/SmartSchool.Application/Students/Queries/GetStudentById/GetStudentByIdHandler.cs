using AutoMapper;
using FluentResults;
using MediatR;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Application.Students.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchool.Application.Students.Queries.GetStudentById
{
    public class GetStudentByIdHandler:IRequestHandler<GetStudentByIdQuery,Result<StudentDto>>
    {
        private readonly IStudentRepository _repo;
        private readonly IMapper _mapper;

        public GetStudentByIdHandler(IStudentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<StudentDto>> Handle(GetStudentByIdQuery request, CancellationToken ct) {
            var student = await _repo.GetByIdAsync(request.Id, ct);

            if (student == null) return Result.Fail("Student not found");

            return Result.Ok(_mapper.Map<StudentDto>(student));
        }
    }
}
