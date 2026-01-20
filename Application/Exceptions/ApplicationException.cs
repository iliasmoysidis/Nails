namespace Application.Exceptions;

public sealed class ApplicationLayerException : Exception
{
    public ApplicationLayerException(string message) : base(message) { }
}