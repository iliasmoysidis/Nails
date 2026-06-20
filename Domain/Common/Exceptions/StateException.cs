namespace Domain.Common.Exceptions;

public sealed class StateException : DomainException
{
    public StateException(string message) : base(message) { }

    public StateException(string message, Exception innerException) : base(message, innerException) { }
}