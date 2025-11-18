using Asidocente.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asidocente.Infrastructure.Persistence.Configurations;

public class AcademicPeriodConfiguration : IEntityTypeConfiguration<AcademicPeriod>
{
    public void Configure(EntityTypeBuilder<AcademicPeriod> builder)
    {
        builder.ToTable("academic_periods");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(a => a.PeriodType).HasColumnName("period_type").IsRequired();
        builder.Property(a => a.SchoolYear).HasColumnName("school_year").IsRequired();
        builder.Property(a => a.PeriodNumber).HasColumnName("period_number").IsRequired();
        builder.Property(a => a.StartDate).HasColumnName("start_date").IsRequired();
        builder.Property(a => a.EndDate).HasColumnName("end_date").IsRequired();
        builder.Property(a => a.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(a => a.SchoolId).HasColumnName("school_id").IsRequired();
        builder.Property(a => a.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(a => a.UpdatedAt).HasColumnName("updated_at");
        builder.Property(a => a.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        builder.Property(a => a.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
        builder.Property(a => a.UpdatedBy).HasColumnName("updated_by").HasMaxLength(50);

        builder.HasOne(a => a.School).WithMany(s => s.AcademicPeriods).HasForeignKey(a => a.SchoolId).OnDelete(DeleteBehavior.Restrict);
        builder.Ignore(a => a.DomainEvents);
    }
}
