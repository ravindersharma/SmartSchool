using Serilog;
using SmartSchool.Api.Configurations;
using SmartSchool.Api.Endpoints;
using SmartSchool.Api.Middlewares;
using SmartSchool.Application;
using SmartSchool.Domain.Enums;
using SmartSchool.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------------
// Logging (Serilog) from appsettings.json
// -----------------------------------------------------------------------------
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration).WriteTo.Console());

// -----------------------------------------------------------------------------
// Core Application Layers
// -----------------------------------------------------------------------------
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// -----------------------------------------------------------------------------
// Authentication + Authorization
// -----------------------------------------------------------------------------
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = "Bearer";
    opts.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer("Bearer", opts =>
{
    opts.SaveToken = true;
    opts.TokenValidationParameters = JwtConfig.GetTokenValidationParameters(builder.Configuration);
});

builder.Services.AddAuthorization(opts =>
{

    opts.AddPolicy("AdminOnly", policy => policy.RequireRole(Role.Admin.ToString()));

    opts.AddPolicy("TeacherOrAdmin", policy => policy.RequireRole(Role.Admin.ToString(), Role.Teacher.ToString()));

});


// -----------------------------------------------------------------------------
// OpenAPI / Swagger
// -----------------------------------------------------------------------------
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<ExceptionMiddleware>();
builder.Services.AddTransient<LoggingMiddleware>();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();
}


// -----------------------------------------------------------------------------
// Middleware injected through UseMiddleware<T> does not need DI registration unless it has constructor dependencies other than RequestDelegate
// -----------------------------------------------------------------------------
//builder.Services.AddTransient<ExceptionMiddleware>();
//builder.Services.AddTransient<LoggingMiddleware>();

// -----------------------------------------------------------------------------
// Build App
// -----------------------------------------------------------------------------
var app = builder.Build();

// 1) OpenAPI Only in Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = string.Empty;
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}


// -----------------------------------------------------------------------------
// MIDDLEWARE ORDER (DO NOT BREAK)
// -----------------------------------------------------------------------------

// 1) GLOBAL EXCEPTION HANDLER (MUST COME FIRST)
app.UseMiddleware<ExceptionMiddleware>();

// 2) REQUEST LOGGING (before auth, after exception)
app.UseMiddleware<LoggingMiddleware>();

// 3) HTTPS
app.UseHttpsRedirection();

// 4) SECURITY PIPELINE
app.UseAuthentication();
app.UseAuthorization();

// -----------------------------------------------------------------------------
// ENDPOINTS
// -----------------------------------------------------------------------------

app.MapAuthEndpoints();
app.MapStudentEndpoints();

// -----------------------------------------------------------------------------
// RUN
// -----------------------------------------------------------------------------
app.Run();

