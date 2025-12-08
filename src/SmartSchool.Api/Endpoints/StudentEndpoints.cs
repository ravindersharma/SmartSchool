using Mapster;
using MediatR;
using SmartSchool.Application.Students.Commands.CreateStudent;
using SmartSchool.Application.Students.Commands.DeleteStudentCommand;
using SmartSchool.Application.Students.Commands.UpdateStudent;
using SmartSchool.Application.Students.Dtos;
using SmartSchool.Application.Students.Queries.GetPagedStudents;
using SmartSchool.Application.Students.Queries.GetStudentById;

namespace SmartSchool.Api.Endpoints
{
    public static class StudentEndpoints
    {
        public static void MapStudentEndpoints(this IEndpointRouteBuilder app)
        {

            var group = app.MapGroup("/api/students").WithTags("Students");
            //Create
            group.MapPost("/", async (CreateStudentCommand cmd, IMediator mediator) =>
            {
                var result = await mediator.Send(cmd);
                return result.IsSuccess ?
                Results.Created($"/api/students/{result.Value.Id}", result.Value) :
                Results.BadRequest(result.Errors);
            }).WithName("CreateStudent")
            .WithSummary("Creates a new student")
            .WithDescription("Creates a student and returns the newly created resource.")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("TeacherOrAdmin");

            //Get By Id
            group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetStudentByIdQuery(id));
                return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
            }).WithName("GetStudentById")
            .WithSummary("Get a student by id")
            .WithDescription("Returns a student if found; otherwise NotFound")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization();

            //Paged record
            group.MapGet("/paged", async (int page, int pageSize, IMediator mediator) =>
            {
                if (page == 0) page = 1;
                if (pageSize == 0) pageSize = 20;
                var result = await mediator.Send(new GetPagedStudentsQuery(page, pageSize));

                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
            }).WithName("GetPagedStudents")
            .WithSummary("Get students paged")
            .WithDescription("Return a list of student using pagination")
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);


            //Update
            group.MapPut("/{id:guid}", async (Guid id, UpdateStudentDto dto, IMediator mediator) =>
            {
                var cmd = dto.Adapt<UpdateStudentCommand>() with { Id = id };
                var resut = await mediator.Send(cmd);
                return resut.IsSuccess ? Results.Ok(resut.Value) : Results.NotFound(resut.Errors);

            }).WithName("UpdateStudent")
            .WithSummary("Update student")
            .WithDescription("Update existing student by Id")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("TeacherOrAdmin");


            //Delete
            group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var cmd = new DeleteStudentCommand(id);
                var result = await mediator.Send(cmd);
                return result.IsSuccess ? Results.Ok() : Results.NotFound(result.Errors);
            }).WithName("DeleteStudent")
            .WithSummary("Delete student")
            .WithDescription("Delete existing student by Id")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("AdminOnly");
        }
    }
}
