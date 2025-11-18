# Asidocente API - Architecture Documentation

## Table of Contents
1. [Overview](#overview)
2. [Project Structure](#project-structure)
3. [Data Flow: Request to Response](#data-flow-request-to-response)
4. [How to Add New Features](#how-to-add-new-features)
5. [Testing Strategy](#testing-strategy)
6. [Deployment Process](#deployment-process)
7. [Design Patterns & Principles](#design-patterns--principles)
8. [Technology Stack](#technology-stack)

---

## Overview

Asidocente API is a school management system built with .NET 8, following **Clean Architecture** principles with **CQRS** pattern. The system is designed for Costa Rican private K-12 schools, managing students, teachers, grades, attendance, and parent relationships.

### Architectural Principles

- **Clean Architecture**: Separation of concerns with clear layer boundaries
- **Domain-Driven Design (DDD)**: Rich domain models with encapsulated business logic
- **CQRS**: Command Query Responsibility Segregation using MediatR
- **Dependency Inversion**: Dependencies point inward toward the domain
- **Testability**: All layers designed for easy unit and integration testing

### Key Technologies

- **.NET 8.0** - Latest LTS framework
- **Entity Framework Core 8.0** - ORM with PostgreSQL
- **MediatR 12** - CQRS implementation
- **FluentValidation 11** - Request validation
- **AutoMapper 12** - Object-to-object mapping
- **xUnit** - Testing framework
- **Docker Compose** - Container orchestration

---

## Project Structure

### Solution Organization

```
Asidocente-App-Api/
├── src/                                    # Source code
│   ├── Asidocente.Domain/                 # Core business logic (no dependencies)
│   ├── Asidocente.Application/            # Business use cases (CQRS)
│   ├── Asidocente.Infrastructure/         # Data access & external services
│   ├── Asidocente.Api/                    # API endpoints & presentation
│   └── Asidocente.Shared/                 # Shared utilities
│
├── tests/                                  # Test projects
│   ├── Asidocente.Application.Tests/      # Unit tests
│   └── Asidocente.Api.IntegrationTests/   # Integration tests
│
├── docker-compose.yml                      # Container orchestration
├── global.json                             # .NET SDK version
├── Asidocente.Institucional.sln           # Solution file
└── README.md                               # Project documentation
```

### Layer Responsibilities

#### 1. Domain Layer (`Asidocente.Domain`)
**Purpose**: Core business logic and domain models
**Dependencies**: None (pure C#)
**Location**: `src/Asidocente.Domain/`

```
Asidocente.Domain/
├── Common/
│   ├── BaseEntity.cs              # Base class with audit & events
│   ├── IAuditableEntity.cs        # Audit tracking interface
│   ├── IDomainEvent.cs            # Domain event marker
│   └── DomainException.cs         # Domain-specific exceptions
│
├── Entities/                       # Core domain entities
│   ├── Student.cs                 # Student aggregate
│   ├── Parent.cs                  # Parent/guardian entity
│   ├── Teacher.cs                 # Teacher entity
│   ├── Subject.cs                 # Academic subject
│   ├── Section.cs                 # Class sections
│   ├── Grade.cs                   # Student grades
│   ├── Attendance.cs              # Attendance records
│   ├── School.cs                  # School aggregate root
│   └── AcademicPeriod.cs         # Terms/semesters
│
├── Enums/                         # Domain enumerations
│   ├── GradeLevel.cs             # K-12 grade levels
│   ├── AttendanceStatus.cs       # Present, Absent, Late, etc.
│   ├── PeriodType.cs             # Trimester, Semester, etc.
│   └── UserRole.cs               # Admin, Teacher, Parent
│
├── Events/                        # Domain events
│   ├── StudentCreatedEvent.cs
│   ├── GradeRegisteredEvent.cs
│   └── AttendanceRecordedEvent.cs
│
└── ValueObjects/                  # Value objects (future)
```

**Key Characteristics**:
- Entities have factory methods (e.g., `Student.Create()`)
- Business rules enforced in entity methods
- Domain events raised for important actions
- No infrastructure concerns (databases, APIs)

#### 2. Application Layer (`Asidocente.Application`)
**Purpose**: Business use cases and orchestration
**Dependencies**: Domain layer, MediatR, FluentValidation, AutoMapper
**Location**: `src/Asidocente.Application/`

```
Asidocente.Application/
├── DependencyInjection.cs         # Service registration
│
├── Common/
│   ├── Behaviours/                # MediatR pipeline behaviors
│   │   ├── ValidationBehaviour.cs  # Auto-validation
│   │   └── LoggingBehaviour.cs     # Performance logging
│   │
│   ├── Exceptions/                # Application exceptions
│   │   ├── ValidationException.cs
│   │   └── NotFoundException.cs
│   │
│   ├── Interfaces/                # Abstraction contracts
│   │   ├── IApplicationDbContext.cs
│   │   ├── ICurrentUserService.cs
│   │   ├── IDateTimeService.cs
│   │   ├── IEmailService.cs
│   │   └── INotificationService.cs
│   │
│   ├── Mappings/
│   │   └── MappingProfile.cs      # AutoMapper profiles
│   │
│   └── Models/
│       ├── Result.cs              # Result pattern
│       └── PaginatedList.cs       # Pagination support
│
└── Features/                      # CQRS features (vertical slices)
    ├── Students/
    │   ├── Commands/
    │   │   └── CreateStudent/
    │   │       ├── CreateStudentCommand.cs
    │   │       ├── CreateStudentCommandHandler.cs
    │   │       └── CreateStudentCommandValidator.cs
    │   ├── Queries/
    │   │   ├── GetStudent/
    │   │   │   ├── GetStudentQuery.cs
    │   │   │   └── GetStudentQueryHandler.cs
    │   │   └── GetStudentsList/
    │   │       ├── GetStudentsListQuery.cs
    │   │       └── GetStudentsListQueryHandler.cs
    │   └── DTOs/
    │       └── StudentDto.cs
    │
    ├── Grades/
    ├── Attendance/
    └── Parents/
```

**Key Characteristics**:
- Vertical slice architecture (feature folders)
- Commands for write operations
- Queries for read operations
- Validators for each command
- DTOs for data transfer
- Pipeline behaviors for cross-cutting concerns

#### 3. Infrastructure Layer (`Asidocente.Infrastructure`)
**Purpose**: Data access, external services, and infrastructure concerns
**Dependencies**: Application, Domain, EF Core, Npgsql
**Location**: `src/Asidocente.Infrastructure/`

```
Asidocente.Infrastructure/
├── DependencyInjection.cs         # Infrastructure registration
│
├── Persistence/
│   ├── ApplicationDbContext.cs    # EF Core context
│   └── Configurations/            # Entity configurations
│       ├── StudentConfiguration.cs
│       ├── ParentConfiguration.cs
│       ├── GradeConfiguration.cs
│       ├── AttendanceConfiguration.cs
│       ├── TeacherConfiguration.cs
│       ├── SubjectConfiguration.cs
│       ├── SectionConfiguration.cs
│       ├── SchoolConfiguration.cs
│       └── AcademicPeriodConfiguration.cs
│
└── Services/                      # Service implementations
    ├── DateTimeService.cs         # DateTime abstraction
    ├── EmailService.cs            # SendGrid integration (TODO)
    └── NotificationService.cs     # Firebase FCM (TODO)
```

**Key Characteristics**:
- Entity Framework Core configurations
- PostgreSQL database provider
- Fluent API for entity mapping
- Service implementations
- Database migrations (future)
- Audit tracking in SaveChangesAsync

#### 4. API Layer (`Asidocente.Api`)
**Purpose**: HTTP endpoints, request/response handling
**Dependencies**: Application, Infrastructure, Swagger
**Location**: `src/Asidocente.Api/`

```
Asidocente.Api/
├── Program.cs                     # Application startup
├── appsettings.json              # Configuration
├── appsettings.Development.json  # Dev settings
│
├── Controllers/                   # API endpoints
│   ├── StudentsController.cs
│   ├── GradesController.cs
│   ├── AttendanceController.cs
│   └── ParentsController.cs
│
├── Middlewares/
│   └── ExceptionHandlerMiddleware.cs  # Global exception handling
│
└── Services/
    └── CurrentUserService.cs      # User context
```

**Key Characteristics**:
- Thin controllers (delegate to MediatR)
- REST API conventions
- Swagger/OpenAPI documentation
- CORS configuration
- Health checks
- Middleware pipeline

#### 5. Shared Layer (`Asidocente.Shared`)
**Purpose**: Utilities and constants shared across projects
**Dependencies**: None
**Location**: `src/Asidocente.Shared/`

```
Asidocente.Shared/
├── Constants/
│   └── ApplicationConstants.cs
└── Extensions/
    ├── DateTimeExtensions.cs
    └── StringExtensions.cs
```

---

## Data Flow: Request to Response

### Complete Request Pipeline

```
┌─────────────────────────────────────────────────────────────────┐
│                        CLIENT REQUEST                            │
│                     (HTTP POST/GET/PUT/DELETE)                   │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                      1. API LAYER (Controllers)                  │
│  Location: src/Asidocente.Api/Controllers/                      │
│                                                                  │
│  • Receives HTTP request                                        │
│  • Deserializes JSON to Command/Query object                    │
│  • No business logic (thin controller)                          │
│                                                                  │
│  Example:                                                        │
│  [HttpPost]                                                      │
│  public async Task<IActionResult> Create(                       │
│      [FromBody] CreateStudentCommand command)                   │
│  {                                                               │
│      var result = await _mediator.Send(command);                │
│      return result.IsSuccess ? Ok(result.Value)                 │
│          : BadRequest(result.Errors);                           │
│  }                                                               │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                   2. MEDIATR (IMediator.Send)                    │
│  Library: MediatR 12                                            │
│                                                                  │
│  • Routes command/query to appropriate handler                  │
│  • Executes pipeline behaviors                                  │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│              3. VALIDATION BEHAVIOR (Pipeline)                   │
│  Location: src/Asidocente.Application/Common/Behaviours/        │
│  File: ValidationBehaviour.cs                                   │
│                                                                  │
│  • Runs all FluentValidation validators for request type       │
│  • Collects validation errors                                   │
│  • Throws ValidationException if errors found                   │
│  • Short-circuits pipeline if validation fails                  │
│                                                                  │
│  Example Validation:                                             │
│  RuleFor(v => v.FirstName)                                      │
│      .NotEmpty()                                                 │
│      .MaximumLength(100);                                       │
│  RuleFor(v => v.Email)                                          │
│      .EmailAddress()                                             │
│      .When(v => !string.IsNullOrWhiteSpace(v.Email));          │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│               4. LOGGING BEHAVIOR (Pipeline)                     │
│  Location: src/Asidocente.Application/Common/Behaviours/        │
│  File: LoggingBehaviour.cs                                      │
│                                                                  │
│  • Logs request details                                         │
│  • Starts performance timer                                     │
│  • Logs execution time after handler completes                  │
│  • Logs errors with stack trace                                 │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│              5. COMMAND/QUERY HANDLER (Business Logic)          │
│  Location: src/Asidocente.Application/Features/[Feature]/       │
│  Example: CreateStudentCommandHandler.cs                        │
│                                                                  │
│  • Receives validated command/query                             │
│  • Performs business logic validation                           │
│  • Interacts with domain entities                               │
│  • Coordinates with DbContext                                   │
│                                                                  │
│  Handler Flow:                                                   │
│  ┌──────────────────────────────────────────┐                  │
│  │ 1. Validate business rules               │                  │
│  │    (e.g., does school exist?)            │                  │
│  ├──────────────────────────────────────────┤                  │
│  │ 2. Create/update domain entities         │                  │
│  │    (using factory methods)               │                  │
│  ├──────────────────────────────────────────┤                  │
│  │ 3. Add to DbContext                      │                  │
│  ├──────────────────────────────────────────┤                  │
│  │ 4. Save changes                          │                  │
│  ├──────────────────────────────────────────┤                  │
│  │ 5. Map to DTO (AutoMapper)               │                  │
│  ├──────────────────────────────────────────┤                  │
│  │ 6. Return Result<T>                      │                  │
│  └──────────────────────────────────────────┘                  │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                  6. APPLICATION DB CONTEXT                       │
│  Location: src/Asidocente.Infrastructure/Persistence/           │
│  File: ApplicationDbContext.cs                                  │
│  Interface: IApplicationDbContext                               │
│                                                                  │
│  • EF Core DbContext                                            │
│  • Exposes DbSet<TEntity> for each entity                       │
│  • Tracks entity changes                                        │
│  • Handles SaveChangesAsync                                     │
│                                                                  │
│  SaveChangesAsync Flow:                                          │
│  ┌──────────────────────────────────────────┐                  │
│  │ 1. Update audit fields                   │                  │
│  │    (CreatedBy, UpdatedBy, timestamps)    │                  │
│  ├──────────────────────────────────────────┤                  │
│  │ 2. Collect domain events                 │                  │
│  ├──────────────────────────────────────────┤                  │
│  │ 3. Call base.SaveChangesAsync()          │                  │
│  ├──────────────────────────────────────────┤                  │
│  │ 4. Dispatch domain events                │                  │
│  ├──────────────────────────────────────────┤                  │
│  │ 5. Clear events from entities            │                  │
│  └──────────────────────────────────────────┘                  │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                     7. ENTITY CONFIGURATIONS                     │
│  Location: src/Asidocente.Infrastructure/Persistence/           │
│           Configurations/                                        │
│  Example: StudentConfiguration.cs                               │
│                                                                  │
│  • Fluent API configuration                                     │
│  • Table and column mappings                                    │
│  • Relationships and foreign keys                               │
│  • Indexes and constraints                                      │
│  • Data type specifications                                     │
│                                                                  │
│  Example:                                                        │
│  builder.Property(s => s.FirstName)                             │
│      .HasColumnName("first_name")                               │
│      .HasMaxLength(100)                                         │
│      .IsRequired();                                             │
│                                                                  │
│  builder.HasIndex(s => s.Identification)                        │
│      .IsUnique();                                               │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                    8. POSTGRESQL DATABASE                        │
│  Provider: Npgsql.EntityFrameworkCore.PostgreSQL                │
│  Version: PostgreSQL 16                                         │
│                                                                  │
│  • Persists entity data                                         │
│  • Enforces constraints                                         │
│  • Executes SQL commands                                        │
│  • Returns results to EF Core                                   │
│                                                                  │
│  Connection Features:                                            │
│  • Retry on failure (5 retries, 30s max delay)                 │
│  • Connection pooling                                            │
│  • ACID transactions                                             │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼ (Results flow back up)
┌─────────────────────────────────────────────────────────────────┐
│                      9. AUTOMAPPER MAPPING                       │
│  Location: src/Asidocente.Application/Common/Mappings/          │
│  File: MappingProfile.cs                                        │
│                                                                  │
│  • Maps domain entities to DTOs                                 │
│  • Transforms data for presentation                             │
│  • Flattens complex object graphs                               │
│  • Computes derived properties                                  │
│                                                                  │
│  Example:                                                        │
│  CreateMap<Student, StudentDto>()                               │
│      .ForMember(d => d.FullName,                                │
│          opt => opt.MapFrom(s => s.GetFullName()))              │
│      .ForMember(d => d.Age,                                     │
│          opt => opt.MapFrom(s => s.GetAge()))                   │
│      .ForMember(d => d.SchoolName,                              │
│          opt => opt.MapFrom(s => s.School.Name));               │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                   10. RESULT PATTERN RESPONSE                    │
│  Location: src/Asidocente.Application/Common/Models/            │
│  File: Result.cs                                                │
│                                                                  │
│  • Encapsulates success/failure state                           │
│  • Includes value or error messages                             │
│  • Avoids exceptions for business failures                      │
│                                                                  │
│  Structure:                                                      │
│  public class Result<T> {                                       │
│      public bool IsSuccess { get; }                             │
│      public T? Value { get; }                                   │
│      public string[] Errors { get; }                            │
│                                                                  │
│      public static Result<T> Success(T value)                   │
│      public static Result<T> Failure(params string[] errors)    │
│  }                                                               │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                11. CONTROLLER RESPONSE FORMATTING                │
│  Location: src/Asidocente.Api/Controllers/                      │
│                                                                  │
│  • Checks Result.IsSuccess                                      │
│  • Returns appropriate HTTP status code                         │
│  • Formats response body                                        │
│                                                                  │
│  Success Response (200/201):                                     │
│  {                                                               │
│      "id": 123,                                                  │
│      "firstName": "Juan",                                        │
│      "lastName": "Pérez",                                        │
│      ...                                                         │
│  }                                                               │
│                                                                  │
│  Error Response (400):                                           │
│  {                                                               │
│      "errors": [                                                 │
│          "First name is required",                              │
│          "Email is invalid"                                      │
│      ]                                                           │
│  }                                                               │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                    12. CLIENT RECEIVES RESPONSE                  │
│                        (JSON over HTTP)                          │
└─────────────────────────────────────────────────────────────────┘
```

### Exception Handling Flow

```
┌──────────────────────────────────────────────────────────┐
│                     EXCEPTION OCCURS                      │
│  (ValidationException, NotFoundException, Exception)      │
└────────────────────────┬─────────────────────────────────┘
                         │
                         ▼
┌──────────────────────────────────────────────────────────┐
│            EXCEPTION HANDLER MIDDLEWARE                   │
│  Location: src/Asidocente.Api/Middlewares/               │
│  File: ExceptionHandlerMiddleware.cs                     │
│                                                           │
│  • Catches all unhandled exceptions                      │
│  • Maps exception type to HTTP status                    │
│  • Formats error response                                │
│  • Logs error details                                    │
│  • Returns JSON error to client                          │
│                                                           │
│  Mapping:                                                 │
│  • ValidationException → 400 Bad Request                 │
│  • NotFoundException → 404 Not Found                     │
│  • DomainException → 400 Bad Request                     │
│  • Exception → 500 Internal Server Error                 │
└────────────────────────┬─────────────────────────────────┘
                         │
                         ▼
┌──────────────────────────────────────────────────────────┐
│                  ERROR RESPONSE TO CLIENT                 │
│  {                                                        │
│      "message": "Validation failed",                     │
│      "errors": {                                         │
│          "firstName": ["First name is required"],        │
│          "email": ["Email is invalid"]                   │
│      }                                                    │
│  }                                                        │
└──────────────────────────────────────────────────────────┘
```

### Example: Creating a Student (Complete Flow)

**1. HTTP Request**
```http
POST /api/students HTTP/1.1
Content-Type: application/json

{
  "firstName": "Juan",
  "lastName": "Pérez",
  "identification": "123456789",
  "gradeLevel": 8,
  "dateOfBirth": "2010-05-15",
  "schoolId": 1,
  "email": "juan.perez@example.com",
  "phone": "88887777",
  "parentIds": [1, 2]
}
```

**2. Controller (StudentsController.cs)**
```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateStudentCommand command)
{
    var result = await _mediator.Send(command);
    return result.IsSuccess
        ? CreatedAtAction(nameof(Get), new { id = result.Value }, result.Value)
        : BadRequest(new { errors = result.Errors });
}
```

**3. Validation (CreateStudentCommandValidator.cs)**
```csharp
RuleFor(v => v.FirstName).NotEmpty().MaximumLength(100);
RuleFor(v => v.Identification).NotEmpty().MaximumLength(20);
RuleFor(v => v.Email).EmailAddress().When(v => !string.IsNullOrWhiteSpace(v.Email));
// ... passes validation
```

**4. Handler (CreateStudentCommandHandler.cs)**
```csharp
public async Task<Result<int>> Handle(CreateStudentCommand request, CancellationToken ct)
{
    // 1. Check if school exists
    var schoolExists = await _context.Schools.AnyAsync(s => s.Id == request.SchoolId, ct);
    if (!schoolExists)
        return Result<int>.Failure("School not found");

    // 2. Check for duplicate identification
    var duplicate = await _context.Students
        .AnyAsync(s => s.Identification == request.Identification, ct);
    if (duplicate)
        return Result<int>.Failure("Student with this identification already exists");

    // 3. Create student using factory method
    var student = Student.Create(
        request.FirstName,
        request.LastName,
        request.Identification,
        (GradeLevel)request.GradeLevel,
        request.DateOfBirth,
        request.SchoolId,
        request.Email,
        request.Phone,
        request.Address
    );

    // 4. Link parents if provided
    if (request.ParentIds?.Any() == true)
    {
        var parents = await _context.Parents
            .Where(p => request.ParentIds.Contains(p.Id))
            .ToListAsync(ct);

        foreach (var parent in parents)
            student.Parents.Add(parent);
    }

    // 5. Save to database
    _context.Students.Add(student);
    await _context.SaveChangesAsync(ct);

    return Result<int>.Success(student.Id);
}
```

**5. Database Persistence**
```sql
-- EF Core generates and executes:
INSERT INTO students (
    first_name, last_name, identification, grade_level,
    date_of_birth, school_id, email, phone,
    created_at, is_active, is_deleted
)
VALUES (
    'Juan', 'Pérez', '123456789', 8,
    '2010-05-15', 1, 'juan.perez@example.com', '88887777',
    '2025-11-18 10:30:00', true, false
)
RETURNING id;

-- Then inserts into many-to-many table
INSERT INTO student_parents (students_id, parents_id)
VALUES (123, 1), (123, 2);
```

**6. HTTP Response**
```http
HTTP/1.1 201 Created
Location: /api/students/123
Content-Type: application/json

123
```

---

## How to Add New Features

This section provides step-by-step guidance for adding new features using the CQRS pattern.

### Feature Development Checklist

- [ ] Define domain entities (if new)
- [ ] Create command/query classes
- [ ] Implement validators
- [ ] Implement handlers
- [ ] Create DTOs
- [ ] Add AutoMapper profiles
- [ ] Create controller endpoints
- [ ] Write unit tests
- [ ] Write integration tests
- [ ] Update API documentation

### Step-by-Step: Adding a New Feature

Let's walk through adding a **"Get Attendance by Date Range"** feature.

#### Step 1: Create Query Class

**Location**: `src/Asidocente.Application/Features/Attendance/Queries/GetAttendanceByDateRange/`

**File**: `GetAttendanceByDateRangeQuery.cs`
```csharp
using MediatR;
using Asidocente.Application.Common.Models;
using Asidocente.Application.Features.Attendance.DTOs;

namespace Asidocente.Application.Features.Attendance.Queries.GetAttendanceByDateRange;

public record GetAttendanceByDateRangeQuery : IRequest<Result<List<AttendanceDto>>>
{
    public int StudentId { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}
```

#### Step 2: Create Query Validator

**File**: `GetAttendanceByDateRangeQueryValidator.cs`
```csharp
using FluentValidation;

namespace Asidocente.Application.Features.Attendance.Queries.GetAttendanceByDateRange;

public class GetAttendanceByDateRangeQueryValidator : AbstractValidator<GetAttendanceByDateRangeQuery>
{
    public GetAttendanceByDateRangeQueryValidator()
    {
        RuleFor(v => v.StudentId)
            .GreaterThan(0)
            .WithMessage("Student ID must be greater than 0");

        RuleFor(v => v.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required");

        RuleFor(v => v.EndDate)
            .NotEmpty()
            .WithMessage("End date is required")
            .GreaterThanOrEqualTo(v => v.StartDate)
            .WithMessage("End date must be after or equal to start date");
    }
}
```

#### Step 3: Create Query Handler

**File**: `GetAttendanceByDateRangeQueryHandler.cs`
```csharp
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Asidocente.Application.Common.Interfaces;
using Asidocente.Application.Common.Models;
using Asidocente.Application.Features.Attendance.DTOs;

namespace Asidocente.Application.Features.Attendance.Queries.GetAttendanceByDateRange;

public class GetAttendanceByDateRangeQueryHandler
    : IRequestHandler<GetAttendanceByDateRangeQuery, Result<List<AttendanceDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAttendanceByDateRangeQueryHandler> _logger;

    public GetAttendanceByDateRangeQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ILogger<GetAttendanceByDateRangeQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<List<AttendanceDto>>> Handle(
        GetAttendanceByDateRangeQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Retrieving attendance for student {StudentId} from {StartDate} to {EndDate}",
            request.StudentId, request.StartDate, request.EndDate);

        // Check if student exists
        var studentExists = await _context.Students
            .AnyAsync(s => s.Id == request.StudentId && !s.IsDeleted, cancellationToken);

        if (!studentExists)
        {
            _logger.LogWarning("Student with ID {StudentId} not found", request.StudentId);
            return Result<List<AttendanceDto>>.Failure("Student not found");
        }

        // Retrieve attendance records
        var attendances = await _context.Attendances
            .Include(a => a.Teacher)
            .AsNoTracking()
            .Where(a => a.StudentId == request.StudentId
                     && a.AttendanceDate >= request.StartDate
                     && a.AttendanceDate <= request.EndDate)
            .OrderBy(a => a.AttendanceDate)
            .ToListAsync(cancellationToken);

        var attendanceDtos = _mapper.Map<List<AttendanceDto>>(attendances);

        _logger.LogInformation(
            "Retrieved {Count} attendance records for student {StudentId}",
            attendanceDtos.Count, request.StudentId);

        return Result<List<AttendanceDto>>.Success(attendanceDtos);
    }
}
```

#### Step 4: Update DTO (if needed)

**Location**: `src/Asidocente.Application/Features/Attendance/DTOs/`

**File**: `AttendanceDto.cs` (ensure it exists)
```csharp
namespace Asidocente.Application.Features.Attendance.DTOs;

public class AttendanceDto
{
    public int Id { get; set; }
    public DateTime AttendanceDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime? ArrivalTime { get; set; }
    public int StudentId { get; set; }
    public int TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public bool WasPresent { get; set; }
    public bool IsExcused { get; set; }
}
```

#### Step 5: Update AutoMapper Profile

**Location**: `src/Asidocente.Application/Common/Mappings/`

**File**: `MappingProfile.cs`
```csharp
// Add to existing CreateMap calls
CreateMap<Attendance, AttendanceDto>()
    .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
    .ForMember(d => d.TeacherName, opt => opt.MapFrom(s => s.Teacher != null
        ? $"{s.Teacher.FirstName} {s.Teacher.LastName}"
        : null))
    .ForMember(d => d.WasPresent, opt => opt.MapFrom(s => s.WasPresent()))
    .ForMember(d => d.IsExcused, opt => opt.MapFrom(s => s.IsExcused()));
```

#### Step 6: Add Controller Endpoint

**Location**: `src/Asidocente.Api/Controllers/`

**File**: `AttendanceController.cs`
```csharp
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Asidocente.Application.Features.Attendance.Queries.GetAttendanceByDateRange;

namespace Asidocente.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly IMediator _mediator;

    public AttendanceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get attendance records for a student within a date range
    /// </summary>
    /// <param name="studentId">The student ID</param>
    /// <param name="startDate">Start date of range</param>
    /// <param name="endDate">End date of range</param>
    /// <returns>List of attendance records</returns>
    [HttpGet("student/{studentId}")]
    [ProducesResponseType(typeof(List<AttendanceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByDateRange(
        int studentId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var query = new GetAttendanceByDateRangeQuery
        {
            StudentId = studentId,
            StartDate = startDate,
            EndDate = endDate
        };

        var result = await _mediator.Send(query);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { errors = result.Errors });
    }

    // ... other endpoints
}
```

#### Step 7: Write Unit Tests

**Location**: `tests/Asidocente.Application.Tests/Features/Attendance/Queries/`

**File**: `GetAttendanceByDateRangeQueryHandlerTests.cs`
```csharp
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using AutoMapper;
using Asidocente.Application.Features.Attendance.Queries.GetAttendanceByDateRange;
using Asidocente.Infrastructure.Persistence;
using Asidocente.Domain.Entities;
using Asidocente.Domain.Enums;

namespace Asidocente.Application.Tests.Features.Attendance.Queries;

public class GetAttendanceByDateRangeQueryHandlerTests
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<GetAttendanceByDateRangeQueryHandler>> _loggerMock;
    private readonly GetAttendanceByDateRangeQueryHandler _handler;

    public GetAttendanceByDateRangeQueryHandlerTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options, null);

        // Setup AutoMapper
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = mapperConfig.CreateMapper();

        // Setup logger mock
        _loggerMock = new Mock<ILogger<GetAttendanceByDateRangeQueryHandler>>();

        _handler = new GetAttendanceByDateRangeQueryHandler(
            _context,
            _mapper,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ValidQuery_ReturnsAttendanceRecords()
    {
        // Arrange
        var school = School.Create("Test School", "TS001", "Province",
            "Canton", "District", "88888888", "school@test.com", "Director");
        _context.Schools.Add(school);
        await _context.SaveChangesAsync();

        var student = Student.Create("Juan", "Pérez", "123456789",
            GradeLevel.Octavo, DateTime.Now.AddYears(-14), school.Id,
            null, null, null);
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        var teacher = Teacher.Create("Maria", "González", "987654321",
            "maria@test.com", "88887777", school.Id, "Math");
        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();

        var attendance1 = Domain.Entities.Attendance.Create(
            DateTime.Now.AddDays(-5),
            AttendanceStatus.Present,
            student.Id,
            teacher.Id);
        var attendance2 = Domain.Entities.Attendance.Create(
            DateTime.Now.AddDays(-3),
            AttendanceStatus.Present,
            student.Id,
            teacher.Id);

        _context.Attendances.AddRange(attendance1, attendance2);
        await _context.SaveChangesAsync();

        var query = new GetAttendanceByDateRangeQuery
        {
            StudentId = student.Id,
            StartDate = DateTime.Now.AddDays(-7),
            EndDate = DateTime.Now
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Should().BeInAscendingOrder(a => a.AttendanceDate);
    }

    [Fact]
    public async Task Handle_NonExistentStudent_ReturnsFailure()
    {
        // Arrange
        var query = new GetAttendanceByDateRangeQuery
        {
            StudentId = 999,
            StartDate = DateTime.Now.AddDays(-7),
            EndDate = DateTime.Now
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain("Student not found");
    }

    [Fact]
    public async Task Validate_InvalidDateRange_ReturnsValidationError()
    {
        // Arrange
        var validator = new GetAttendanceByDateRangeQueryValidator();
        var query = new GetAttendanceByDateRangeQuery
        {
            StudentId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(-7) // End before start
        };

        // Act
        var result = await validator.ValidateAsync(query);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("End date must be after or equal to start date"));
    }
}
```

#### Step 8: Write Integration Tests

**Location**: `tests/Asidocente.Api.IntegrationTests/Controllers/`

**File**: `AttendanceControllerTests.cs`
```csharp
using System.Net;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Asidocente.Api.IntegrationTests.Controllers;

public class AttendanceControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AttendanceControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetByDateRange_ValidRequest_ReturnsOk()
    {
        // Arrange
        var studentId = 1; // Assuming seeded data
        var startDate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
        var endDate = DateTime.Now.ToString("yyyy-MM-dd");

        // Act
        var response = await _client.GetAsync(
            $"/api/attendance/student/{studentId}?startDate={startDate}&endDate={endDate}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<List<AttendanceDto>>();
        content.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByDateRange_InvalidDateRange_ReturnsBadRequest()
    {
        // Arrange
        var studentId = 1;
        var startDate = DateTime.Now.ToString("yyyy-MM-dd");
        var endDate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");

        // Act
        var response = await _client.GetAsync(
            $"/api/attendance/student/{studentId}?startDate={startDate}&endDate={endDate}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
```

### Adding a Command (Write Operation)

For commands (CREATE, UPDATE, DELETE operations), follow the same pattern but create in the `Commands/` folder:

**Structure:**
```
Features/[Feature]/Commands/[CommandName]/
├── [CommandName].cs              # Command class
├── [CommandName]Handler.cs       # Handler implementation
└── [CommandName]Validator.cs     # Validation rules
```

**Example Command:**
```csharp
// UpdateStudentCommand.cs
public record UpdateStudentCommand : IRequest<Result<Unit>>
{
    public int Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
}

// UpdateStudentCommandValidator.cs
public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {
        RuleFor(v => v.Id).GreaterThan(0);
        RuleFor(v => v.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(v => v.LastName).NotEmpty().MaximumLength(100);
        RuleFor(v => v.Email).EmailAddress()
            .When(v => !string.IsNullOrWhiteSpace(v.Email));
    }
}

// UpdateStudentCommandHandler.cs
public class UpdateStudentCommandHandler
    : IRequestHandler<UpdateStudentCommand, Result<Unit>>
{
    private readonly IApplicationDbContext _context;

    public async Task<Result<Unit>> Handle(
        UpdateStudentCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Students
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (student == null)
            return Result<Unit>.Failure("Student not found");

        student.UpdateInfo(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}
```

### Quick Reference: Command vs Query

| Aspect | Command (Write) | Query (Read) |
|--------|----------------|--------------|
| **Purpose** | Modify data | Retrieve data |
| **Folder** | `Commands/` | `Queries/` |
| **Return Type** | `Result<T>` or `Result<Unit>` | `Result<TDto>` or `Result<List<TDto>>` |
| **DbContext** | Tracking enabled | `.AsNoTracking()` |
| **Validation** | Always validated | Validated if needed |
| **Side Effects** | Yes (database changes) | No (read-only) |
| **Examples** | Create, Update, Delete | Get, List, Search |

---

## Testing Strategy

### Testing Pyramid

```
        ┌─────────────────┐
        │  E2E Tests      │  Few, slow, expensive
        │  (Minimal)      │
        ├─────────────────┤
        │  Integration    │  Some, moderate speed
        │  Tests          │
        ├─────────────────┤
        │  Unit Tests     │  Many, fast, cheap
        │                 │
        └─────────────────┘
```

### Test Projects

#### 1. Unit Tests (`Asidocente.Application.Tests`)

**Location**: `tests/Asidocente.Application.Tests/`

**Purpose**: Test business logic in isolation

**Technologies**:
- xUnit - Test framework
- FluentAssertions - Readable assertions
- Moq - Mocking dependencies
- EF Core InMemory - In-memory database

**What to Test**:
- Command handlers
- Query handlers
- Validators
- Domain entity methods
- Mapping profiles
- Business rules

**Example Test Structure**:
```csharp
public class CreateStudentCommandHandlerTests
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly CreateStudentCommandHandler _handler;

    public CreateStudentCommandHandlerTests()
    {
        // Arrange: Setup in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _context = new ApplicationDbContext(options, _currentUserServiceMock.Object);
        _handler = new CreateStudentCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesStudent()
    {
        // Arrange
        var school = School.Create(...);
        _context.Schools.Add(school);
        await _context.SaveChangesAsync();

        var command = new CreateStudentCommand
        {
            FirstName = "Juan",
            LastName = "Pérez",
            // ... other properties
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeGreaterThan(0);

        var student = await _context.Students.FindAsync(result.Value);
        student.Should().NotBeNull();
        student.FirstName.Should().Be("Juan");
    }

    [Fact]
    public async Task Handle_DuplicateIdentification_ReturnsFailure()
    {
        // Arrange
        var school = School.Create(...);
        _context.Schools.Add(school);

        var existingStudent = Student.Create(
            "Existing", "Student", "123456789",
            GradeLevel.Octavo, DateTime.Now.AddYears(-14), school.Id);
        _context.Students.Add(existingStudent);
        await _context.SaveChangesAsync();

        var command = new CreateStudentCommand
        {
            Identification = "123456789", // Duplicate
            // ... other properties
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain("identification already exists");
    }
}
```

**Running Unit Tests**:
```bash
# Run all unit tests
dotnet test tests/Asidocente.Application.Tests/

# Run with code coverage
dotnet test tests/Asidocente.Application.Tests/ --collect:"XPlat Code Coverage"

# Run specific test
dotnet test --filter "FullyQualifiedName~CreateStudentCommandHandlerTests"
```

#### 2. Integration Tests (`Asidocente.Api.IntegrationTests`)

**Location**: `tests/Asidocente.Api.IntegrationTests/`

**Purpose**: Test complete request/response flow

**Technologies**:
- xUnit
- WebApplicationFactory
- TestServer

**What to Test**:
- HTTP endpoints
- Request/response serialization
- Middleware pipeline
- Database integration
- Error handling

**Example Test Structure**:
```csharp
public class StudentsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public StudentsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateStudent_ValidRequest_Returns201Created()
    {
        // Arrange
        var request = new
        {
            firstName = "Juan",
            lastName = "Pérez",
            identification = "123456789",
            gradeLevel = 8,
            dateOfBirth = "2010-05-15",
            schoolId = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/students", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var studentId = await response.Content.ReadFromJsonAsync<int>();
        studentId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CreateStudent_InvalidEmail_Returns400BadRequest()
    {
        // Arrange
        var request = new
        {
            firstName = "Juan",
            lastName = "Pérez",
            identification = "123456789",
            email = "invalid-email", // Invalid format
            gradeLevel = 8,
            dateOfBirth = "2010-05-15",
            schoolId = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/students", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Email");
    }
}
```

**Running Integration Tests**:
```bash
# Run all integration tests
dotnet test tests/Asidocente.Api.IntegrationTests/

# Run with verbose output
dotnet test tests/Asidocente.Api.IntegrationTests/ --logger "console;verbosity=detailed"
```

### Test Coverage Goals

| Layer | Coverage Target | Priority |
|-------|----------------|----------|
| Domain Entities | 90%+ | High |
| Command Handlers | 85%+ | High |
| Query Handlers | 80%+ | High |
| Validators | 90%+ | High |
| Controllers | 70%+ | Medium |
| Middleware | 70%+ | Medium |
| Services | 75%+ | Medium |

### Test Naming Convention

Use the pattern: `MethodName_Scenario_ExpectedResult`

**Examples**:
- `Handle_ValidCommand_CreatesStudent`
- `Handle_DuplicateIdentification_ReturnsFailure`
- `Validate_EmptyFirstName_ReturnsValidationError`
- `Create_NullDateOfBirth_ThrowsDomainException`

### Continuous Testing

**During Development**:
```bash
# Watch mode (re-runs on file changes)
dotnet watch test --project tests/Asidocente.Application.Tests/
```

**Pre-Commit Hook** (recommended):
```bash
#!/bin/bash
# .git/hooks/pre-commit

dotnet test --no-build --logger "console;verbosity=minimal"

if [ $? -ne 0 ]; then
    echo "Tests failed. Commit aborted."
    exit 1
fi
```

**CI/CD Pipeline**:
```yaml
# Example GitHub Actions
- name: Run Unit Tests
  run: dotnet test tests/Asidocente.Application.Tests/ --no-build

- name: Run Integration Tests
  run: dotnet test tests/Asidocente.Api.IntegrationTests/ --no-build

- name: Generate Coverage Report
  run: dotnet test --collect:"XPlat Code Coverage"
```

---

## Deployment Process

### Environment Setup

#### Development
```json
// appsettings.Development.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=asidocente_dev;Username=postgres;Password=dev_password"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

#### Staging
```json
// appsettings.Staging.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=staging-db.example.com;Database=asidocente_staging;Username=app_user;Password=${DB_PASSWORD}"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  }
}
```

#### Production
```json
// appsettings.Production.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=${DB_HOST};Database=${DB_NAME};Username=${DB_USER};Password=${DB_PASSWORD};SSL Mode=Require"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.EntityFrameworkCore": "Error"
    }
  }
}
```

### Deployment Methods

#### 1. Docker Deployment (Recommended)

**Step 1: Create Dockerfile**

**Location**: `src/Asidocente.Api/Dockerfile`
```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Asidocente.Institucional.sln", "./"]
COPY ["src/Asidocente.Api/Asidocente.Api.csproj", "src/Asidocente.Api/"]
COPY ["src/Asidocente.Application/Asidocente.Application.csproj", "src/Asidocente.Application/"]
COPY ["src/Asidocente.Domain/Asidocente.Domain.csproj", "src/Asidocente.Domain/"]
COPY ["src/Asidocente.Infrastructure/Asidocente.Infrastructure.csproj", "src/Asidocente.Infrastructure/"]
COPY ["src/Asidocente.Shared/Asidocente.Shared.csproj", "src/Asidocente.Shared/"]

# Restore dependencies
RUN dotnet restore

# Copy remaining source code
COPY . .

# Build application
WORKDIR "/src/src/Asidocente.Api"
RUN dotnet build "Asidocente.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Asidocente.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published app
COPY --from=publish /app/publish .

# Set environment variable
ENV ASPNETCORE_URLS=http://+:8080

# Create non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "Asidocente.Api.dll"]
```

**Step 2: Build and Run with Docker Compose**

```bash
# Build all services
docker-compose build

# Start all services
docker-compose up -d

# View logs
docker-compose logs -f api

# Stop all services
docker-compose down

# Restart API only
docker-compose restart api
```

**Step 3: Apply Database Migrations**

```bash
# Create migration
dotnet ef migrations add InitialCreate --project src/Asidocente.Infrastructure --startup-project src/Asidocente.Api

# Apply migrations to database
docker-compose exec api dotnet ef database update --project /app/Asidocente.Infrastructure.dll

# Or run migrations on startup (add to Program.cs)
```

**Auto-Migration on Startup** (optional):
```csharp
// Program.cs
if (app.Environment.IsDevelopment() || args.Contains("--migrate"))
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
}
```

#### 2. Traditional Deployment (IIS/Nginx)

**Step 1: Publish Application**
```bash
# Publish for production
dotnet publish src/Asidocente.Api/Asidocente.Api.csproj \
    -c Release \
    -o ./publish \
    --self-contained false \
    --runtime linux-x64

# Create deployment package
tar -czf asidocente-api.tar.gz -C ./publish .
```

**Step 2: Setup on Server**
```bash
# Extract on server
mkdir -p /var/www/asidocente-api
tar -xzf asidocente-api.tar.gz -C /var/www/asidocente-api

# Set permissions
chown -R www-data:www-data /var/www/asidocente-api

# Create systemd service
sudo nano /etc/systemd/system/asidocente-api.service
```

**Systemd Service File**:
```ini
[Unit]
Description=Asidocente API
After=network.target

[Service]
Type=notify
WorkingDirectory=/var/www/asidocente-api
ExecStart=/usr/bin/dotnet /var/www/asidocente-api/Asidocente.Api.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=asidocente-api
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

**Step 3: Configure Nginx Reverse Proxy**
```nginx
server {
    listen 80;
    server_name api.asidocente.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
    }
}
```

**Step 4: Enable and Start Service**
```bash
# Reload systemd
sudo systemctl daemon-reload

# Enable on boot
sudo systemctl enable asidocente-api

# Start service
sudo systemctl start asidocente-api

# Check status
sudo systemctl status asidocente-api

# View logs
sudo journalctl -u asidocente-api -f
```

#### 3. Cloud Deployment (Azure/AWS)

**Azure App Service**:
```bash
# Login to Azure
az login

# Create resource group
az group create --name asidocente-rg --location eastus

# Create App Service plan
az appservice plan create \
    --name asidocente-plan \
    --resource-group asidocente-rg \
    --sku B1 \
    --is-linux

# Create web app
az webapp create \
    --name asidocente-api \
    --resource-group asidocente-rg \
    --plan asidocente-plan \
    --runtime "DOTNETCORE:8.0"

# Deploy from local folder
az webapp deploy \
    --name asidocente-api \
    --resource-group asidocente-rg \
    --src-path ./publish \
    --type zip
```

**AWS Elastic Beanstalk**:
```bash
# Install EB CLI
pip install awsebcli

# Initialize EB application
eb init -p "64bit Amazon Linux 2023 v3.0.0 running .NET 8" asidocente-api

# Create environment
eb create asidocente-production

# Deploy
eb deploy

# View logs
eb logs
```

### Database Migration Strategy

#### Development
```bash
# Add migration
dotnet ef migrations add MigrationName \
    --project src/Asidocente.Infrastructure \
    --startup-project src/Asidocente.Api

# Update database
dotnet ef database update \
    --project src/Asidocente.Infrastructure \
    --startup-project src/Asidocente.Api

# Remove last migration (if not applied)
dotnet ef migrations remove \
    --project src/Asidocente.Infrastructure \
    --startup-project src/Asidocente.Api
```

#### Production
```bash
# Generate SQL script (review before applying)
dotnet ef migrations script \
    --project src/Asidocente.Infrastructure \
    --startup-project src/Asidocente.Api \
    --output migration.sql

# Apply manually to production database
psql -h production-db.example.com -U app_user -d asidocente_prod -f migration.sql

# Or use automated migration on startup (cautious approach)
```

### CI/CD Pipeline Example (GitHub Actions)

**Location**: `.github/workflows/deploy.yml`
```yaml
name: Deploy Asidocente API

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build-and-test:
    runs-on: ubuntu-latest

    services:
      postgres:
        image: postgres:16
        env:
          POSTGRES_DB: asidocente_test
          POSTGRES_USER: postgres
          POSTGRES_PASSWORD: test_password
        options: >-
          --health-cmd pg_isready
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5
        ports:
          - 5432:5432

    steps:
    - uses: actions/checkout@v3

    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --no-restore --configuration Release

    - name: Run Unit Tests
      run: dotnet test tests/Asidocente.Application.Tests/ --no-build --configuration Release --logger "trx;LogFileName=test-results.trx"

    - name: Run Integration Tests
      run: dotnet test tests/Asidocente.Api.IntegrationTests/ --no-build --configuration Release
      env:
        ConnectionStrings__DefaultConnection: "Host=localhost;Database=asidocente_test;Username=postgres;Password=test_password"

    - name: Publish
      run: dotnet publish src/Asidocente.Api/Asidocente.Api.csproj -c Release -o ./publish

    - name: Upload artifact
      uses: actions/upload-artifact@v3
      with:
        name: api-package
        path: ./publish

  deploy-to-production:
    runs-on: ubuntu-latest
    needs: build-and-test
    if: github.ref == 'refs/heads/main'

    steps:
    - name: Download artifact
      uses: actions/download-artifact@v3
      with:
        name: api-package
        path: ./publish

    - name: Deploy to Azure
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'asidocente-api'
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ./publish
```

### Health Checks and Monitoring

**Health Check Endpoint**: `/health`

**Response**:
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.1234567",
  "entries": {
    "database": {
      "status": "Healthy",
      "duration": "00:00:00.0987654"
    }
  }
}
```

**Monitoring Setup**:
```csharp
// Program.cs - Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "postgresql")
    .AddRedis(
        builder.Configuration.GetValue<string>("Redis:Connection")!,
        name: "redis");

// Map health check endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
```

### Deployment Checklist

- [ ] Run all tests locally
- [ ] Update version number
- [ ] Generate and review migration scripts
- [ ] Build Docker image (if using containers)
- [ ] Tag release in Git
- [ ] Deploy to staging environment
- [ ] Run smoke tests on staging
- [ ] Apply database migrations to production
- [ ] Deploy application to production
- [ ] Verify health check endpoint
- [ ] Monitor logs for errors
- [ ] Update API documentation
- [ ] Notify team of deployment

### Rollback Procedure

**Docker Deployment**:
```bash
# Tag images before deployment
docker tag asidocente-api:latest asidocente-api:backup-$(date +%Y%m%d-%H%M%S)

# Rollback if needed
docker-compose down
docker tag asidocente-api:backup-20231118-143000 asidocente-api:latest
docker-compose up -d
```

**Database Rollback**:
```bash
# Revert to specific migration
dotnet ef database update PreviousMigrationName \
    --project src/Asidocente.Infrastructure \
    --startup-project src/Asidocente.Api
```

---

## Design Patterns & Principles

### Architectural Patterns

#### 1. Clean Architecture
- **Dependency Rule**: Dependencies point inward
- **Domain Layer**: Independent, no external dependencies
- **Application Layer**: Depends only on Domain
- **Infrastructure Layer**: Implements interfaces from Application
- **API Layer**: Depends on Application and Infrastructure

#### 2. CQRS (Command Query Responsibility Segregation)
- **Commands**: Modify state, return success/failure
- **Queries**: Read data, use `.AsNoTracking()`
- **Handlers**: Single responsibility, one command/query per handler
- **Benefits**: Scalability, clear intent, optimized queries

#### 3. Domain-Driven Design (DDD)
- **Entities**: Identity-based objects (Student, Grade)
- **Value Objects**: Immutable, compared by value (future)
- **Aggregates**: Consistency boundaries (Student + Parents)
- **Domain Events**: Decouple domain actions
- **Factory Methods**: Encapsulate creation logic

#### 4. Repository Pattern
- **Interface**: `IApplicationDbContext` abstracts data access
- **Implementation**: `ApplicationDbContext` uses EF Core
- **Benefits**: Testability, swappable data sources

#### 5. Mediator Pattern
- **Library**: MediatR
- **Purpose**: Decouples sender from receiver
- **Benefits**: Single Responsibility, testable pipelines
- **Behaviors**: Cross-cutting concerns (validation, logging)

### SOLID Principles

#### Single Responsibility Principle
- Each handler does one thing
- Validators separate from handlers
- Controllers delegate to MediatR

#### Open/Closed Principle
- Extend via new handlers, not modifying existing code
- Pipeline behaviors add functionality without changing handlers

#### Liskov Substitution Principle
- Interfaces enable swappable implementations
- `IApplicationDbContext` can be mocked or replaced

#### Interface Segregation Principle
- Small, focused interfaces (IEmailService, IDateTimeService)
- Clients depend only on methods they use

#### Dependency Inversion Principle
- High-level modules depend on abstractions
- Infrastructure implements interfaces defined in Application

### Other Design Patterns

#### Result Pattern
- Explicit success/failure handling
- Avoids exceptions for business rule violations
- Type-safe error handling

#### Pipeline Pattern
- MediatR behaviors create processing pipeline
- Validation → Logging → Handler → Response
- Extensible and testable

#### Factory Method Pattern
- `Student.Create()` encapsulates creation logic
- Ensures valid initial state
- Raises domain events on creation

---

## Technology Stack

### Core Framework
- **.NET 8.0** - Latest LTS, improved performance
- **C# 12** - Modern language features

### Data Access
- **Entity Framework Core 8.0** - ORM
- **Npgsql 8.0** - PostgreSQL provider
- **PostgreSQL 16** - Relational database

### CQRS & Validation
- **MediatR 12** - Mediator pattern implementation
- **FluentValidation 11** - Fluent validation rules

### Mapping & Serialization
- **AutoMapper 12** - Object-to-object mapping
- **System.Text.Json** - JSON serialization

### Authentication & Security
- **Microsoft.AspNetCore.Identity** (planned)
- **JWT Bearer Authentication** (planned)

### Caching & Messaging
- **StackExchange.Redis 2.8** - Redis client
- **Redis 7** - In-memory cache (optional)

### API Documentation
- **Swashbuckle.AspNetCore 6.6** - Swagger/OpenAPI
- **Microsoft.AspNetCore.OpenApi 8.0** - OpenAPI support

### Testing
- **xUnit 2.5** - Test framework
- **FluentAssertions 6.12** - Readable assertions
- **Moq 4.20** - Mocking framework
- **Coverlet 6.0** - Code coverage

### Infrastructure
- **Docker** - Containerization
- **Docker Compose** - Multi-container orchestration
- **Nginx** (optional) - Reverse proxy

### External Services (Planned)
- **SendGrid** - Email notifications
- **Firebase Cloud Messaging** - Push notifications

---

## Additional Resources

### File Locations Quick Reference

| Component | Location |
|-----------|----------|
| Solution File | `Asidocente.Institucional.sln` |
| API Startup | `src/Asidocente.Api/Program.cs` |
| DbContext | `src/Asidocente.Infrastructure/Persistence/ApplicationDbContext.cs` |
| Domain Entities | `src/Asidocente.Domain/Entities/` |
| CQRS Features | `src/Asidocente.Application/Features/` |
| Controllers | `src/Asidocente.Api/Controllers/` |
| Entity Configs | `src/Asidocente.Infrastructure/Persistence/Configurations/` |
| Unit Tests | `tests/Asidocente.Application.Tests/` |
| Integration Tests | `tests/Asidocente.Api.IntegrationTests/` |
| Docker Compose | `docker-compose.yml` |

### Common Commands

```bash
# Build solution
dotnet build

# Run API locally
dotnet run --project src/Asidocente.Api

# Run tests
dotnet test

# Create migration
dotnet ef migrations add MigrationName --project src/Asidocente.Infrastructure --startup-project src/Asidocente.Api

# Update database
dotnet ef database update --project src/Asidocente.Infrastructure --startup-project src/Asidocente.Api

# Generate migration script
dotnet ef migrations script --project src/Asidocente.Infrastructure --startup-project src/Asidocente.Api --output migration.sql

# Start with Docker
docker-compose up -d

# View logs
docker-compose logs -f api

# Stop services
docker-compose down
```

### API Documentation

When running locally, access Swagger UI at:
- **URL**: http://localhost:5000 (redirects to Swagger in Development)
- **Swagger UI**: http://localhost:5000/swagger
- **OpenAPI JSON**: http://localhost:5000/swagger/v1/swagger.json

### Support and Contact

For questions or issues:
1. Check existing documentation
2. Review code comments
3. Consult domain entity methods
4. Refer to handler implementations
5. Contact development team

---

**Document Version**: 1.0
**Last Updated**: 2025-11-18
**Maintained By**: Development Team
