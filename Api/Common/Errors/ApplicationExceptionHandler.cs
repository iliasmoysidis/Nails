using Application.Common.Exceptions;
using Domain.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Common.Errors;

public sealed class ApplicationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken ct
    )
    {
        var (statusCode, title) = exception switch
        {
            ApplicationLayerNotFoundException or NotFoundException
                => (StatusCodes.Status404NotFound, "Not Found"),

            ApplicationLayerForbiddenException
                => (StatusCodes.Status403Forbidden, "Forbidden"),

            ApplicationLayerValidationException or ValidationException
                => (StatusCodes.Status400BadRequest, "Validation Failed"),

            InvariantException or StateException
                => (StatusCodes.Status409Conflict, "Conflict"),

            ApplicationLayerException or DomainException
                => (StatusCodes.Status400BadRequest, "Bad Request"),

            _ => (0, string.Empty)
        };

        if (statusCode == 0)
            return false;

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message
            },
            ct
        );

        return true;
    }
}
