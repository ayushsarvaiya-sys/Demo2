using Backend.Data;
using Backend.Mappings;
using Backend.Repositories.Implementations;
using Backend.Repositories.Interfaces;
using Backend.Services.Implementations;
using Backend.Services.Interfaces;
using Backend.Utiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy => policy.WithOrigins("http://localhost:4200", "http://127.0.0.1:4200", "https://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Repository Dependency Injection
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

// Service Dependency Injection
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

// Model Validation => ModelState error handeling
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        return new BadRequestObjectResult(new ApiError(
            (int)HttpStatusCode.BadRequest,
            "Validation Failed",
            errors
        ));
    };
});

// Mapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AppProfile>();
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS must come before UseHttpsRedirection
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
