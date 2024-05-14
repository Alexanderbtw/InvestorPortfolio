using Application.Services;
using InvestorAPI.Contracts.User;
using Npgsql;

namespace InvestorAPI.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("register", Register);
        group.MapPost("login", Login);
        return group;
    }
    private static async Task<IResult> Register(
        HttpContext context, 
        UserService userService,
        RegisterUserRequest request)
    {
        try
        {
            await userService.Register(request.Username, request.Email, request.Password);
        }
        catch (PostgresException e)
        {
            return Results.BadRequest("User already exist");
        }
        
        return Results.Ok();
    }

    private static async Task<IResult> Login(
        HttpContext context, 
        UserService userService,
        LoginUserRequest request)
    {
        try
        {
            var token = await userService.Login(request.Email, request.Password);
            context.Response.Cookies.Append("token", token);
        }
        catch (NullReferenceException e)
        {
            return Results.NotFound(e.Message);
        }
        catch (ArgumentNullException e)
        {
            return Results.BadRequest(e.Message);
        }
        return Results.Ok();
    }
}