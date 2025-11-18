using Asidocente.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asidocente.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for School
/// </summary>
public class SchoolConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder.ToTable("schools");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(s => s.Code).HasColumnName("code").HasMaxLength(20).IsRequired();
        builder.Property(s => s.Province).HasColumnName("province").HasMaxLength(100);
        builder.Property(s => s.Canton).HasColumnName("canton").HasMaxLength(100);
        builder.Property(s => s.District).HasColumnName("district").HasMaxLength(100);
        builder.Property(s => s.DetailedAddress).HasColumnName("detailed_address").HasMaxLength(500);
        builder.Property(s => s.Phone).HasColumnName("phone").HasMaxLength(20);
        builder.Property(s => s.Email).HasColumnName("email").HasMaxLength(100);
        builder.Property(s => s.Director).HasColumnName("director").HasMaxLength(200);
        builder.Property(s => s.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(s => s.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(s => s.UpdatedAt).HasColumnName("updated_at");
        builder.Property(s => s.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        builder.Property(s => s.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
        builder.Property(s => s.UpdatedBy).HasColumnName("updated_by").HasMaxLength(50);

        builder.HasIndex(s => s.Code).IsUnique().HasDatabaseName("ix_schools_code");

        builder.Ignore(s => s.DomainEvents);
    }
}
