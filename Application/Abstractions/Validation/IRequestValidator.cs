namespace Application.Abstractions.Validation;

public interface IRequestValidator<in TRequest>
{
    Task ValidateAsync(TRequest request, CancellationToken ct);
}