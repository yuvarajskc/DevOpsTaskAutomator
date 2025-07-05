using System.Net;
using FluentValidation;
using System.Text.Json;
using DevOpsTaskApp.Application.Common.Exceptions;

namespace DevOpsTaskApp.WebAPI.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = "Validation failed",
                errors = ex.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = ex.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception)
        {
            // Optional: handle other exceptions (500)
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync("{\"message\": \"An unexpected error occurred\"}");
        }
    }
}
