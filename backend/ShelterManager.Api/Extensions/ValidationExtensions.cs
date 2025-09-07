using ShelterManager.Api.Filters;

namespace ShelterManager.Api.Extensions;

public static class ValidationExtensions
{
    public static RouteHandlerBuilder WithRequestValidation<T>(this RouteHandlerBuilder builder)
        where T : class
    {
        return builder.AddEndpointFilter<ValidationFilter<T>>()
            .ProducesValidationProblem();
    }
}