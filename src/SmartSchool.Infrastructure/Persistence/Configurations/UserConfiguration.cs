using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Domain.Entities;
using SmartSchool.Domain.Enums;

namespace SmartSchool.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            base.Configure(builder);
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x=>x.Email).IsRequired().HasMaxLength(200);
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x=>x.Role).IsRequired().HasConversion<String>().HasMaxLength(50);
            builder.Property(x=>x.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            //builder.Property(x => x.IsDeleted).HasDefaultValue(false);

            //builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
