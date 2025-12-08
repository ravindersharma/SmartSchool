using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Application.Auth.Commands.ForgotPassword;
using SmartSchool.Application.Auth.Commands.Login;
using SmartSchool.Application.Auth.Commands.RefreshToken;
using SmartSchool.Application.Auth.Commands.RegisterUser;
using SmartSchool.Application.Auth.Commands.ResetPassword;
using SmartSchool.Application.Auth.Commands.RevokeToken;
using SmartSchool.Application.Auth.Dtos;

namespace SmartSchool.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        //Register
        group.MapPost("/register", async (RegisterRequestDto req, IMediator mediator, HttpContext ctx) =>
        {
            var origin = ctx.Request.Headers["Origin"].ToString();
            var result = await mediator.Send(new RegisterCommand(req, origin));
            return result.IsSuccess ?
            Results.Created($"/api/users/{result.Value.UserId}", result.Value) :
            Results.BadRequest(result.Errors);
        })
        .AllowAnonymous()
        .WithName("RegisterUser")
        .WithSummary("Register a new user")
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);


        //Login
        group.MapPost("/login", async (LoginRequestDto req, IMediator mediator, HttpContext ctx) =>
        {
            var ipAddress = ctx.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
            var result = await mediator.Send(new LoginCommand(req, ipAddress));
            return result.IsSuccess ?
            Results.Ok(result.Value) :
            Results.BadRequest(result.Errors);
        }).AllowAnonymous()
        .WithName("LoginUser")
        .WithSummary("Login a user")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest); ;

        //Refresh Token
        group.MapPost("/refresh", async ([FromBody] string token, IMediator mediator, HttpContext ctx) =>
        {
            var ipAddress = ctx.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
            var result = await mediator.Send(new RefreshTokenCommand(token, ipAddress));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        }).AllowAnonymous()
        .WithName("RefrehToken")
        .WithSummary("Refresh JWT using refresh token")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest); ;

        //Forgot Password
        group.MapPost("/forgot", async ([FromBody] string email, IMediator mediator, HttpContext ctx) =>
        {
            var origin = ctx.Request.Headers["Origin"].ToString();
            var result = await mediator.Send(new ForgotPasswordCommand(email, origin));
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
        }).AllowAnonymous()
        .WithName("ForgotPassword")
        .WithSummary("Request password reset")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);


        //Reset Password
        group.MapPost("/reset", async ([FromBody] ResetPasswordRequestDto req, IMediator mediator, HttpContext ctx) =>
        {
            var origin = ctx.Request.Headers["Origin"].ToString();
            var result = await mediator.Send(new ResetPasswordCommand(req, origin));
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
        }).AllowAnonymous()
        .WithName("ResetPassword")
        .WithSummary("Reset password using token")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest); ;

        //revoke
        group.MapPost("/revoke", async (string refreshToken, HttpContext ctx, ISender mediator) =>
        {
            var ip = ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var command = new RevokeTokenCommand(refreshToken, ip);

            var result = await mediator.Send(command);

            if (result.IsFailed)
                return Results.BadRequest(result.Errors.Select(e => e.Message));

            return Results.Ok(new { message = "Token revoked successfully." });

        })
        .WithName("RevokeToken")
        .WithSummary("Revoke refresh token")
        .WithDescription("Revokes a refresh token so that it cannot be used again.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

    }
}