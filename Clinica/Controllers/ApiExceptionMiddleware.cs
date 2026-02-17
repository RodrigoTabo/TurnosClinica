namespace TurnosClinica.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidOperationException ex)
            {
                await WriteProblem(context, StatusCodes.Status409Conflict, ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                await WriteProblem(context, StatusCodes.Status404NotFound, ex.Message);
            }
            catch (ArgumentException ex)
            {
                await WriteProblem(context, StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        private static Task WriteProblem(HttpContext context, int status, string message)
        {
            context.Response.StatusCode = status;
            return context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = message,
                Status = status
            });
        }
    }


}
