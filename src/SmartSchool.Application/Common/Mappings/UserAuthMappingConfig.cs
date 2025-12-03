using Mapster;
using SmartSchool.Application.Auth.Dtos;
using SmartSchool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchool.Application.Common.Mappings
{
    public class UserAuthMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Add your mapping configurations here
            config.NewConfig<User, AuthResponseDto>()
                .Map(dest => dest.UserId, src => src.Id)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.UserName, src => src.UserName)
                .Map(dest => dest.Role, src => src.Role);
        }
    }
}
