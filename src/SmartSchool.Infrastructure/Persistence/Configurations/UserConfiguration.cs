using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Domain.Entities;
using SmartSchool.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchool.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x=>x.Email).IsRequired().HasMaxLength(200);
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x=>x.Role).IsRequired().HasConversion<Role>().HasMaxLength(50);
        }
    }
}
