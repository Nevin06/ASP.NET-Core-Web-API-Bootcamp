using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DemoProject;
//54
public class ExceptionMiddleware
{
    //Request delegate to call the next middleware
    private RequestDelegate Next { get; }
    public ExceptionMiddleware(RequestDelegate next)
    { Next = next; }
    //Inject HttpContext
    //First Middleware and it invokes other middlewares
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await Next(httpContext);
        }
        //Custom exception
        catch (InvalidNameException ex)
        {
            httpContext.Response.ContentType= "application/problem+json";
            httpContext.Response.StatusCode = 400;
            var problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = string.Empty,
                Instance = string.Empty,
                Title = "Name is invalid",
                Type = "Error"
            };
            
            var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
            await httpContext.Response.WriteAsync(problemDetailsJson);
        }
        catch (Exception ex)
        {
            httpContext.Response.ContentType = "application/problem+json";
            httpContext.Response.StatusCode = 500;
            var problemDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message,
                Instance = string.Empty,
                Title = "Internal Server Error - something went wrong",
                Type = "Error"
            };

            var problemDetailsJson = JsonSerializer.Serialize(problemDetails);
            await httpContext.Response.WriteAsync(problemDetailsJson);
        }
    }
}
