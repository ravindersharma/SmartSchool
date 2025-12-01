using FluentResults;
using MapsterMapper;
using MediatR;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Application.Students.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchool.Application.Students.Queries.GetPagedStudents
{
    public class GetPagedStudentsHandler : IRequestHandler<GetPagedStudentsQuery, Result<IEnumerable<StudentDto>>>
    {
        private readonly IStudentRepository _repo;
        private readonly IMapper _mapper;

        public GetPagedStudentsHandler(IStudentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        public async Task<Result<IEnumerable<StudentDto>>> Handle(GetPagedStudentsQuery request, CancellationToken ct)
        {

            var data = await _repo.GetPagedAsync(request.Page, request.PageSize, ct);

            return Result.Ok(_mapper.Map<IEnumerable<StudentDto>>(data));
        }
    }
}
