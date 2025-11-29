using MediatR;
using SmartSchool.Application.Students.Commands.CreateStudent;
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
            .WithDescription("Creates a student and returns the newly created resource.");

            //Get By Id
            group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetStudentByIdQuery(id));
                return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
            }).WithName("GetStudentById")
            .WithSummary("Get a student by id")
            .WithDescription("Returns a student if found; otherwise NotFound");

        }
    }
}
