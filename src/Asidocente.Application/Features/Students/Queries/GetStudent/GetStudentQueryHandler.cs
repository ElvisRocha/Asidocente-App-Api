using Asidocente.Application.Common.Interfaces;
using Asidocente.Application.Common.Models;
using Asidocente.Application.Features.Students.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Asidocente.Application.Features.Students.Queries.GetStudent;

/// <summary>
/// Handler for GetStudentQuery
/// </summary>
public class GetStudentQueryHandler : IRequestHandler<GetStudentQuery, Result<StudentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetStudentQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<StudentDto>> Handle(GetStudentQuery request, CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .Include(s => s.School)
            .Include(s => s.Section)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (student == null)
        {
            return Result<StudentDto>.Failure("Student not found");
        }

        var studentDto = _mapper.Map<StudentDto>(student);
        return Result<StudentDto>.Success(studentDto);
    }
}
