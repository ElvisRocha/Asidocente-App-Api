using Asidocente.Api.Middlewares;
using Asidocente.Api.Services;
using Asidocente.Application;
using Asidocente.Application.Common.Interfaces;
using Asidocente.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add Application and Infrastructure services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add API-specific services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Asidocente API",
        Version = "v1",
        Description = "School Management System API for Costa Rican Private K-12 Schools",
        Contact = new OpenApiContact
        {
            Name = "Asidocente",
            Email = "support@asidocente.com"
        }
    });

    // Enable XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<Asidocente.Infrastructure.Persistence.ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline

// Use custom exception handler middleware
app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Asidocente API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at app's root
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

// Welcome endpoint
app.MapGet("/", () => new
{
    name = "Asidocente API",
    version = "1.0.0",
    description = "School Management System for Costa Rican K-12 Schools",
    documentation = "/swagger"
}).WithName("Root").WithOpenApi();

app.Run();
