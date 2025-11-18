using Asidocente.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Asidocente.Application.Common.Interfaces;

/// <summary>
/// Application database context interface
/// </summary>
public interface IApplicationDbContext
{
    DbSet<School> Schools { get; }
    DbSet<Student> Students { get; }
    DbSet<Parent> Parents { get; }
    DbSet<Teacher> Teachers { get; }
    DbSet<Subject> Subjects { get; }
    DbSet<Section> Sections { get; }
    DbSet<Grade> Grades { get; }
    DbSet<Attendance> Attendances { get; }
    DbSet<AcademicPeriod> AcademicPeriods { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
