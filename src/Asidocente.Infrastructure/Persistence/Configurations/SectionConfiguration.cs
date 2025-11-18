using Asidocente.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asidocente.Infrastructure.Persistence.Configurations;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.ToTable("sections");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.Property(s => s.GradeLevel).HasColumnName("grade_level").IsRequired();
        builder.Property(s => s.Capacity).HasColumnName("capacity").IsRequired();
        builder.Property(s => s.SchoolYear).HasColumnName("school_year").IsRequired();
        builder.Property(s => s.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(s => s.SchoolId).HasColumnName("school_id").IsRequired();
        builder.Property(s => s.HomeRoomTeacherId).HasColumnName("homeroom_teacher_id");
        builder.Property(s => s.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(s => s.UpdatedAt).HasColumnName("updated_at");
        builder.Property(s => s.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        builder.Property(s => s.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
        builder.Property(s => s.UpdatedBy).HasColumnName("updated_by").HasMaxLength(50);

        builder.HasOne(s => s.School).WithMany(sc => sc.Sections).HasForeignKey(s => s.SchoolId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(s => s.HomeRoomTeacher).WithMany(t => t.Sections).HasForeignKey(s => s.HomeRoomTeacherId).OnDelete(DeleteBehavior.SetNull);
        builder.Ignore(s => s.DomainEvents);
    }
}
