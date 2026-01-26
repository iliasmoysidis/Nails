namespace Application.Exceptions;

public sealed class NotFoundException : ApplicationLayerException
{
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}