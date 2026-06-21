namespace Application.Common.Exceptions;

public abstract class ApplicationLayerException : Exception
{
    public ApplicationLayerException(string message) : base(message) { }

    public ApplicationLayerException(string message, Exception innerException) : base(message, innerException) { }
}