namespace Application.Common.Abstractions.Validation;

public interface IRequestValidator<in TRequest>
{
    Task ValidateAsync(TRequest request, CancellationToken ct);
}