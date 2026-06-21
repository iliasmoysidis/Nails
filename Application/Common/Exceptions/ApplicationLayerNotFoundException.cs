namespace Application.Common.Exceptions;

public sealed class ApplicationLayerNotFoundException : ApplicationLayerException
{
    public ApplicationLayerNotFoundException(string message) : base(message) { }

    public ApplicationLayerNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}