using Asidocente.Application.Common.Interfaces;
using Asidocente.Application.Features.Grades.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Asidocente.Application.Features.Grades.Queries.GetGradesByStudent;

/// <summary>
/// Handler for GetGradesByStudentQuery
/// </summary>
public class GetGradesByStudentQueryHandler : IRequestHandler<GetGradesByStudentQuery, List<GradeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetGradesByStudentQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<GradeDto>> Handle(GetGradesByStudentQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Grades
            .Include(g => g.Student)
            .Include(g => g.Subject)
            .Include(g => g.AcademicPeriod)
            .AsNoTracking()
            .Where(g => g.StudentId == request.StudentId);

        if (request.AcademicPeriodId.HasValue)
        {
            query = query.Where(g => g.AcademicPeriodId == request.AcademicPeriodId.Value);
        }

        return await query
            .OrderBy(g => g.GradeDate)
            .ProjectTo<GradeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
