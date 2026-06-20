namespace Domain.Common.Exceptions;

public sealed class InvariantException : DomainException
{
    public InvariantException(string message) : base(message) { }

    public InvariantException(string message, Exception innerException) : base(message, innerException) { }
}