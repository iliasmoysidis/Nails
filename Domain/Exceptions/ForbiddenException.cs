namespace Domain.Exceptions;

public sealed class ForbiddenException : DomainException
{
    public ForbiddenException(string message) : base(message) { }

    public ForbiddenException(string message, Exception innerException) : base(message, innerException) { }
}