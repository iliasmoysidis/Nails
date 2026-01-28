namespace Application.Exceptions;

public sealed class ApplicationLayerValidationException : ApplicationLayerException
{
    public ApplicationLayerValidationException(string message) : base(message) { }

    public ApplicationLayerValidationException(string message, Exception innerException) : base(message, innerException) { }
}