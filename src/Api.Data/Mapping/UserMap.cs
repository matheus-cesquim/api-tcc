using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Mapping
{
    public class UserMap : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("user");

            builder.HasKey(column => column.Id);
            builder.Property(column => column.UserName)
                .HasMaxLength(30);
            builder.Property(column => column.Email)
                .HasMaxLength(100);
            builder.Property(column => column.Password)
               .HasMaxLength(60);
            builder.HasIndex(column => column.Email)
                .IsUnique();
            builder.HasIndex(column => column.UserName)
                .IsUnique();
            builder.Property(column => column.Email)
                .IsRequired();
            builder.Property(column => column.UserName)
                .IsRequired();
            builder.Property(column => column.Password)
                .IsRequired();
            builder.Property(column => column.CreateAt)
                .IsRequired();
        }
    }
}
