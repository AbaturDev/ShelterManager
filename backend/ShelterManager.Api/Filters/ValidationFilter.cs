using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ShelterManager.Api.Filters;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
    where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<T>().FirstOrDefault();
        if (request == null)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Invalid request body",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Request body could not be parsed or is missing."
            };

            return TypedResults.Problem(problemDetails);
        }
        
        var result = await validator.ValidateAsync(request, context.HttpContext.RequestAborted);

        if (!result.IsValid)
        {
            var errors = result.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            
            var problemDetails = new ValidationProblemDetails(errors)
            {
                Title = "Validation failed",
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or more validation errors occurred.",
            };
            
            return TypedResults.Problem(problemDetails);
        }
        
        return await next(context);
    }
}