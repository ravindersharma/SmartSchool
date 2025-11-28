using MediatR;
using SmartSchool.Application.Students.Commands.CreateStudent;
using SmartSchool.Application.Students.Queries.GetStudentById;

namespace SmartSchool.Api.Endpoints
{
    public static class StudentEndpoints
    {
        public static void MapStudentEndpoints(this IEndpointRouteBuilder app)
        {

            var group = app.MapGroup("/students");

            group.MapPost("/", async (CreateStudentCommand cmd, IMediator mediator) =>
            {
                var result = await mediator.Send(cmd);
                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
            });

            group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetStudentByIdQuery(id));
                return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
            });

        }
    }
}
