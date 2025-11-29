using Mapster;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Domain.Entities;

namespace SmartSchool.Application.Common.Mappings
{
    public class StudentMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Student, StudentDto>();
        }
    }
}
