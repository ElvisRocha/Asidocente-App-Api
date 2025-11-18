using AutoMapper;
using Asidocente.Domain.Entities;
using Asidocente.Application.Features.Students.DTOs;
using Asidocente.Application.Features.Grades.DTOs;
using Asidocente.Application.Features.Attendance.DTOs;
using Asidocente.Application.Features.Parents.DTOs;

namespace Asidocente.Application.Common.Mappings;

/// <summary>
/// AutoMapper profile for entity to DTO mappings
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Student mappings
        CreateMap<Student, StudentDto>()
            .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.GetFullName()))
            .ForMember(d => d.Age, opt => opt.MapFrom(s => s.GetAge()))
            .ForMember(d => d.SchoolName, opt => opt.MapFrom(s => s.School.Name))
            .ForMember(d => d.SectionName, opt => opt.MapFrom(s => s.Section != null ? s.Section.Name : null));

        CreateMap<Student, StudentListDto>()
            .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.GetFullName()))
            .ForMember(d => d.SchoolName, opt => opt.MapFrom(s => s.School.Name));

        // Parent mappings
        CreateMap<Parent, ParentDto>()
            .ForMember(d => d.FullName, opt => opt.MapFrom(p => p.GetFullName()));

        // Grade mappings
        CreateMap<Grade, GradeDto>()
            .ForMember(d => d.StudentName, opt => opt.MapFrom(g => g.Student.GetFullName()))
            .ForMember(d => d.SubjectName, opt => opt.MapFrom(g => g.Subject.Name))
            .ForMember(d => d.PeriodName, opt => opt.MapFrom(g => g.AcademicPeriod.Name))
            .ForMember(d => d.LetterGrade, opt => opt.MapFrom(g => g.GetLetterGrade()))
            .ForMember(d => d.IsPassing, opt => opt.MapFrom(g => g.IsPassing()));

        CreateMap<Grade, GradeReportDto>()
            .ForMember(d => d.SubjectName, opt => opt.MapFrom(g => g.Subject.Name))
            .ForMember(d => d.LetterGrade, opt => opt.MapFrom(g => g.GetLetterGrade()))
            .ForMember(d => d.IsPassing, opt => opt.MapFrom(g => g.IsPassing()));

        // Attendance mappings
        CreateMap<Attendance, AttendanceDto>()
            .ForMember(d => d.StudentName, opt => opt.MapFrom(a => a.Student.GetFullName()))
            .ForMember(d => d.WasPresent, opt => opt.MapFrom(a => a.WasPresent()))
            .ForMember(d => d.IsExcused, opt => opt.MapFrom(a => a.IsExcused()));

        // Teacher mappings
        CreateMap<Teacher, TeacherDto>()
            .ForMember(d => d.FullName, opt => opt.MapFrom(t => t.GetFullName()));

        // Subject mappings
        CreateMap<Subject, SubjectDto>();

        // Section mappings
        CreateMap<Section, SectionDto>()
            .ForMember(d => d.HomeRoomTeacherName, opt => opt.MapFrom(s => s.HomeRoomTeacher != null ? s.HomeRoomTeacher.GetFullName() : null))
            .ForMember(d => d.EnrolledCount, opt => opt.MapFrom(s => s.Students.Count))
            .ForMember(d => d.AvailableSlots, opt => opt.MapFrom(s => s.GetAvailableSlots()));

        // School mappings
        CreateMap<School, SchoolDto>();

        // AcademicPeriod mappings
        CreateMap<AcademicPeriod, AcademicPeriodDto>()
            .ForMember(d => d.IsCurrentPeriod, opt => opt.MapFrom(p => p.IsCurrentPeriod()));
    }
}

// DTOs for common entities (Teacher, Subject, Section, School, AcademicPeriod)
public record TeacherDto
{
    public int Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string? Specialization { get; init; }
}

public record SubjectDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int Credits { get; init; }
}

public record SectionDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string GradeLevel { get; init; } = string.Empty;
    public int Capacity { get; init; }
    public int EnrolledCount { get; init; }
    public int AvailableSlots { get; init; }
    public int SchoolYear { get; init; }
    public string? HomeRoomTeacherName { get; init; }
}

public record SchoolDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string? Director { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public bool IsActive { get; init; }
}

public record AcademicPeriodDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string PeriodType { get; init; } = string.Empty;
    public int SchoolYear { get; init; }
    public int PeriodNumber { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public bool IsCurrentPeriod { get; init; }
}
