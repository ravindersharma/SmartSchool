using Mapster;
using SmartSchool.Application.Users.Dtos;
using SmartSchool.Domain.Entities;
using SmartSchool.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchool.Application.Common.Mappings
{
    public class UserMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // =========================
            // USER MAPPINGS
            // =========================

            // CreateUserDto → User (without password hash)
            config.NewConfig<CreateUserDto, User>()
                .Map(dest => dest.Id, _ => Guid.NewGuid())
                .Map(dest => dest.Email, src => src.Email.ToLowerInvariant())
                .Map(dest => dest.UserName, src => src.UserName)
                .Map(dest => dest.Role, src => Enum.Parse<Role>(src.Role, true))
                .Map(dest => dest.IsDeleted, _ => false)
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow);

            // User → UserDto
            config.NewConfig<User, UserDto>()
                .Map(dest => dest.Role, src => src.Role.ToString());

        }
    }
}
