using Asidocente.Application.Common.Interfaces;
using Asidocente.Application.Common.Models;
using Asidocente.Application.Features.Students.DTOs;
using Asidocente.Domain.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Asidocente.Application.Features.Students.Queries.GetStudentsList;

/// <summary>
/// Handler for GetStudentsListQuery
/// </summary>
public class GetStudentsListQueryHandler : IRequestHandler<GetStudentsListQuery, PaginatedList<StudentListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetStudentsListQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<StudentListDto>> Handle(GetStudentsListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Students
            .Include(s => s.School)
            .AsNoTracking()
            .AsQueryable();

        // Apply filters
        if (request.SchoolId.HasValue)
        {
            query = query.Where(s => s.SchoolId == request.SchoolId.Value);
        }

        if (request.GradeLevel.HasValue)
        {
            query = query.Where(s => s.GradeLevel == (GradeLevel)request.GradeLevel.Value);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(s => s.IsActive == request.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(s =>
                s.FirstName.ToLower().Contains(searchTerm) ||
                s.LastName.ToLower().Contains(searchTerm) ||
                s.Identification.Contains(searchTerm));
        }

        // Order by name
        query = query.OrderBy(s => s.LastName).ThenBy(s => s.FirstName);

        var mappedQuery = query.ProjectTo<StudentListDto>(_mapper.ConfigurationProvider);

        return await PaginatedList<StudentListDto>.CreateAsync(
            mappedQuery,
            request.PageNumber,
            request.PageSize,
            cancellationToken);
    }
}
