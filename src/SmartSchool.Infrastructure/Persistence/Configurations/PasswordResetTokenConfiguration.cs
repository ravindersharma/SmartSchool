using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Domain.Entities;

namespace SmartSchool.Infrastructure.Persistence.Configurations
{
    public class PasswordResetTokenConfiguration : BaseEntityConfiguration<PasswordResetToken>
    {
        public override void Configure(EntityTypeBuilder<PasswordResetToken> builder)
        {
            builder.ToTable("PasswordResetTokens");
            base.Configure(builder);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Token).IsRequired();
            builder.Property(x => x.ExpiresAt).IsRequired();
            builder.Property(x => x.Used).IsRequired();
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            //builder.Property(x => x.IsDeleted).HasDefaultValue(false);


            //builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
