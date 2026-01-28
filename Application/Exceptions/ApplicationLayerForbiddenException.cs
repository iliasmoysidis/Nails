namespace Application.Exceptions;

public sealed class ApplicationLayerForbiddenException : ApplicationLayerException
{
    public ApplicationLayerForbiddenException(string message) : base(message) { }

    public ApplicationLayerForbiddenException(string message, Exception innerException) : base(message, innerException) { }
}