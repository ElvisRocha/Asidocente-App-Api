using Asidocente.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asidocente.Infrastructure.Persistence.Configurations;

public class ParentConfiguration : IEntityTypeConfiguration<Parent>
{
    public void Configure(EntityTypeBuilder<Parent> builder)
    {
        builder.ToTable("parents");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");
        builder.Property(p => p.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
        builder.Property(p => p.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
        builder.Property(p => p.Identification).HasColumnName("identification").HasMaxLength(20).IsRequired();
        builder.Property(p => p.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
        builder.Property(p => p.Phone).HasColumnName("phone").HasMaxLength(20).IsRequired();
        builder.Property(p => p.AlternatePhone).HasColumnName("alternate_phone").HasMaxLength(20);
        builder.Property(p => p.Occupation).HasColumnName("occupation").HasMaxLength(100);
        builder.Property(p => p.Relationship).HasColumnName("relationship").HasMaxLength(50).IsRequired();
        builder.Property(p => p.IsPrimaryContact).HasColumnName("is_primary_contact").HasDefaultValue(false);
        builder.Property(p => p.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(p => p.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(p => p.UpdatedAt).HasColumnName("updated_at");
        builder.Property(p => p.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        builder.Property(p => p.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
        builder.Property(p => p.UpdatedBy).HasColumnName("updated_by").HasMaxLength(50);
        builder.HasIndex(p => p.Identification).IsUnique().HasDatabaseName("ix_parents_identification");
        builder.Ignore(p => p.DomainEvents);
    }
}
