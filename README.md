# Asidocente API

A comprehensive school management system API for Costa Rican private K-12 schools, built with .NET 8 and Clean Architecture principles.

## 🎯 Project Overview

Asidocente is a modern, scalable school management system designed specifically for private K-12 schools in Costa Rica. This API replaces the previous Supabase backend with a robust .NET 8 solution featuring:

- **Student enrollment and management**
- **Academic grades and performance tracking**
- **Daily attendance monitoring**
- **Parent communication portal**
- **Teacher management**
- **Administrative dashboards**

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
Asidocente-App-Api/
├── src/
│   ├── Asidocente.Api/              # API layer (Controllers, Middleware)
│   ├── Asidocente.Application/       # Business logic (CQRS, Commands, Queries)
│   ├── Asidocente.Domain/            # Core domain models and business rules
│   ├── Asidocente.Infrastructure/    # Data access, external services
│   └── Asidocente.Shared/            # Common utilities and extensions
├── tests/
│   ├── Asidocente.Application.Tests/
│   └── Asidocente.Api.IntegrationTests/
├── docker-compose.yml
└── README.md
```

### Architecture Layers

1. **Domain Layer** - Pure C# with no dependencies
   - Entities (Student, Teacher, Grade, Attendance, etc.)
   - Value Objects (Email, PhoneNumber, Address)
   - Domain Events
   - Business rules and validations

2. **Application Layer** - Business logic and use cases
   - CQRS with MediatR
   - Commands and Queries
   - FluentValidation validators
   - AutoMapper profiles
   - DTOs

3. **Infrastructure Layer** - External concerns
   - Entity Framework Core with PostgreSQL
   - Database configurations
   - External services (Email, Notifications)

4. **API Layer** - HTTP endpoints
   - RESTful controllers
   - Exception handling middleware
   - Swagger/OpenAPI documentation

## 🚀 Technology Stack

- **.NET 8** - Latest LTS version
- **Entity Framework Core 8** - ORM
- **PostgreSQL 16** - Database
- **MediatR** - CQRS pattern
- **FluentValidation** - Request validation
- **AutoMapper** - Object mapping
- **Swagger/OpenAPI** - API documentation
- **xUnit** - Unit testing
- **Docker** - Containerization
- **Redis** - Caching (optional)

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 16](https://www.postgresql.org/download/) or Docker
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional)
- IDE: Visual Studio 2022, VS Code, or JetBrains Rider

## 🔧 Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd Asidocente-App-Api
```

### 2. Set Up the Database

#### Option A: Using Docker (Recommended)

```bash
docker-compose up -d postgres redis
```

#### Option B: Local PostgreSQL

Ensure PostgreSQL is running and create a database:

```sql
CREATE DATABASE asidocente_dev;
```

### 3. Update Connection String

Edit `src/Asidocente.Api/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=asidocente_dev;Username=postgres;Password=YOUR_PASSWORD"
  }
}
```

### 4. Run Database Migrations

```bash
cd src/Asidocente.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../Asidocente.Api
dotnet ef database update --startup-project ../Asidocente.Api
```

### 5. Run the API

```bash
cd src/Asidocente.Api
dotnet run
```

The API will be available at:
- **HTTP**: http://localhost:5000
- **Swagger UI**: http://localhost:5000 (root)

## 🐳 Docker Deployment

### Build and Run All Services

```bash
docker-compose up --build
```

This will start:
- PostgreSQL on port 5432
- Redis on port 6379
- API on port 5000

### Stop Services

```bash
docker-compose down
```

### Stop and Remove Volumes

```bash
docker-compose down -v
```

## 📚 API Documentation

Once the API is running, access the interactive Swagger documentation at:

**http://localhost:5000**

### Main Endpoints

#### Students
- `POST /api/students` - Create a new student
- `GET /api/students/{id}` - Get student by ID
- `GET /api/students` - Get paginated list of students

#### Grades
- `POST /api/grades` - Register a new grade
- `GET /api/grades/student/{studentId}` - Get grades by student

#### Attendance
- `POST /api/attendance` - Record student attendance

#### Parents
- `POST /api/parents` - Create a new parent

## 🧪 Running Tests

### Run All Tests

```bash
dotnet test
```

### Run Specific Test Project

```bash
dotnet test tests/Asidocente.Application.Tests
```

### Run with Coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## 🗄️ Database Migrations

### Create a New Migration

```bash
cd src/Asidocente.Infrastructure
dotnet ef migrations add <MigrationName> --startup-project ../Asidocente.Api
```

### Apply Migrations

```bash
dotnet ef database update --startup-project ../Asidocente.Api
```

### Remove Last Migration

```bash
dotnet ef migrations remove --startup-project ../Asidocente.Api
```

### Generate SQL Script

```bash
dotnet ef migrations script --startup-project ../Asidocente.Api -o migration.sql
```

## 🔑 Configuration

### Environment Variables

Key configuration settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=asidocente_dev;Username=postgres;Password=dev_password"
  },
  "JwtSettings": {
    "Secret": "your-secret-key",
    "Issuer": "Asidocente.Api",
    "Audience": "Asidocente.Client",
    "ExpirationInMinutes": 60
  },
  "Redis": {
    "Connection": "localhost:6379"
  },
  "SendGrid": {
    "ApiKey": "your-sendgrid-api-key"
  },
  "Firebase": {
    "ServerKey": "your-firebase-server-key"
  }
}
```

## 📦 Project Structure Details

### Domain Entities

- **School** - Educational institution
- **Student** - Student information and relationships
- **Parent** - Parent/guardian information
- **Teacher** - Teacher profiles
- **Grade** - Academic grades/scores
- **Attendance** - Daily attendance records
- **Subject** - Academic subjects/courses
- **Section** - Class sections (e.g., "2-A", "3-B")
- **AcademicPeriod** - Trimesters, bimesters, semesters

### Grade Levels (Costa Rican System)

- **Preescolar**: Maternal, PreKinder, Kinder, Preparatoria
- **Primaria**: Primero through Sexto
- **Secundaria**: Séptimo through Duodécimo

## 🔐 Security

- Input validation with FluentValidation
- Parameterized queries (SQL injection protection)
- CORS configuration
- Authentication/Authorization ready (JWT)
- Secure password hashing (when implemented)

## 🚦 Health Checks

The API includes health check endpoints:

- `GET /health` - Database connectivity check

## 📈 Performance

- Asynchronous operations throughout
- Entity Framework query optimization
- Pagination for large datasets
- Optional Redis caching layer
- Connection pooling

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Coding Standards

- Follow C# coding conventions
- Use meaningful variable and method names
- Write XML documentation for public APIs
- Include unit tests for new features
- Follow SOLID principles

## 📝 License

This project is proprietary and confidential.

## 📧 Contact

For support or inquiries:
- Email: support@asidocente.com
- Documentation: http://localhost:5000/swagger

## 🗺️ Roadmap

- [x] Core domain models
- [x] CQRS implementation
- [x] Database integration
- [x] Basic CRUD operations
- [ ] JWT Authentication
- [ ] Role-based authorization
- [ ] Email notifications (SendGrid)
- [ ] Push notifications (Firebase FCM)
- [ ] Report generation (PDF)
- [ ] Dashboard analytics
- [ ] Bulk operations
- [ ] File uploads (student photos, documents)
- [ ] Audit logging
- [ ] Rate limiting

## 🐛 Known Issues

None at this time. Please report issues via GitHub Issues.

## 📚 Additional Resources

- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [MediatR](https://github.com/jbogard/MediatR)
- [FluentValidation](https://docs.fluentvalidation.net/)

---

**Built with ❤️ for Costa Rican educators**
