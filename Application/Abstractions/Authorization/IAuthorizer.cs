namespace Application.Abstractions.Authorization;

public interface IAuthorizer<in TRequest>
{
    Task AuthorizeAsync(TRequest request, CancellationToken ct);
}