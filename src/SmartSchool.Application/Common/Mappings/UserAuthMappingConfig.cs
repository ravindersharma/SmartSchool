using Mapster;
using SmartSchool.Application.Auth.Dtos;
using SmartSchool.Application.Users.Dtos;
using SmartSchool.Domain.Entities;
using SmartSchool.Domain.Enums;

namespace SmartSchool.Application.Common.Mappings
{
    public class UserAuthMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Add your mapping configurations here
            //Getting response from auth service
            config.NewConfig<User, AuthResponseDto>()
                .Map(dest => dest.UserId, src => src.Id)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.UserName, src => src.UserName)
                .Map(dest => dest.Role, src => src.Role.ToString());

            config.NewConfig<RegisterRequestDto, User>()
                    .Map(dest => dest.Role, src => Enum.Parse<Role>(src.Role, true));
        }
    }
}
