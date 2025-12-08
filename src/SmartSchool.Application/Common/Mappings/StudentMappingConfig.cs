using Mapster;
using SmartSchool.Application.Students.Commands.UpdateStudent;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Domain.Entities;

namespace SmartSchool.Application.Common.Mappings
{
    public class StudentMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // =========================
            // STUDENT MAPPINGS
            // =========================

            // CreateStudentDto → Student
            config.NewConfig<CreateStudentDto, Student>()
                .Map(dest => dest.Id, _ => Guid.NewGuid())
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.FullName, src => src.FullName.Trim())
                .Map(dest => dest.Grade, src => src.Grade)
                .Map(dest => dest.DOB, src => src.DOB)
                .Map(dest => dest.Nationality, src => src.Nationality)
                .Map(dest => dest.NationalId, src => src.NationalId)
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Map(dest => dest.IsDeleted, _ => false);

            // Student → StudentDto (includes nested User fields)
            config.NewConfig<Student, StudentDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.Email, src => src.User!.Email)
                .Map(dest => dest.UserName, src => src.User!.UserName)
                .Map(dest => dest.Role, src => src.User!.Role.ToString())
                .Map(dest => dest.FullName, src => src.FullName)
                .Map(dest => dest.Grade, src => src.Grade)
                .Map(dest => dest.DOB, src => src.DOB)
                .Map(dest => dest.Nationality, src => src.Nationality)
                .Map(dest => dest.NationalId, src => src.NationalId);

            // UpdateStudentDto → UpdateStudentCommand id will be set separately
            config.NewConfig<UpdateStudentDto, UpdateStudentCommand>()
                .Map(dest => dest.Dto.FullName, src => src.FullName.Trim())
                .Map(dest => dest.Dto.Grade, src => src.Grade)
                .Map(dest => dest.Dto.DOB, src => src.DOB); 

        }
    }
}
