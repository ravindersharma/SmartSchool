using Serilog;
using SmartSchool.Api.Configurations;
using SmartSchool.Api.Endpoints;
using SmartSchool.Api.Middlewares;
using SmartSchool.Application;
using SmartSchool.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
//Logging
// serilog to get config from appsettings.json
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration).WriteTo.Console());

//Configuration and services
//Layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Authentication + Authrization 
builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = "Bearer";
    opts.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer("Bearer", opts =>
{
    opts.SaveToken = true;
    opts.TokenValidationParameters = AuthConfig.GetTokenValidationParameters(builder.Configuration);
});

builder.Services.AddAuthorization();


//OpenApi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();//optional

//register custom servies or middlerware before use them
builder.Services.AddTransient<ExceptionMiddleware>();

//Build App
var app = builder.Build();

//Swagger or OpenAPI for Development only
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();  // exposes openapi.json
    //optional swagger ui development only
    app.UseSwagger(); // swagger.json
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = string.Empty;   // Load Swagger at "/"
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");    // IMPORTANT: starts with '/'
    });
}

//Order of middlerwares matters so please don't nreak it 

//custom middlewares should be befor http verbs because they break the pipline ,

//so we must place it near the top before any exception happen
app.UseMiddleware<ExceptionMiddleware>();
//https rdeirection
app.UseHttpsRedirection();

// Security
app.UseAuthentication();
app.UseAuthorization();

//Loggine request 
app.UseSerilogRequestLogging();


//Map Endpoints
app.MapStudentEndpoints();



app.Run();

