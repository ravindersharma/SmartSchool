using Serilog;
using SmartSchool.Api.Configurations;
using SmartSchool.Api.Endpoints;
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


//Build App
var app = builder.Build();


//Exception Handling Middleware for prod vs dev
if (app.Environment.IsDevelopment())
{
    //Detailed error in dev
    app.UseDeveloperExceptionPage();
}
else
{
    //Firendly error in prod
    app.UseExceptionHandler("/error");
    //Minimal global exception Handlers
    //app.UseExceptionHandler(errApp =>
    //{
    //    errApp.Run(async (ctx) =>
    //    {
    //        ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
    //        ctx.Response.ContentType = "application/json";
    //        await ctx.Response.WriteAsJsonAsync(new { error = "An unexpected error occured" });
    //    });
    //});
}

//Swagger or OpenAPI for Development only
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();  // exposes openapi.json
    //optional swagger ui development only
    app.UseSwagger(); // swagger.json
    app.UseSwaggerUI();  // UI
}



// Security
app.UseAuthentication();
app.UseAuthorization();

//Loggine request 
app.UseSerilogRequestLogging();
//https rdeirection
app.UseHttpsRedirection();

//Map Endpoints
app.MapStudentEndpoints();

app.Run();

