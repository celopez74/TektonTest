namespace Tekton.Products.Domain.Exception;

public class BadRequestException : ClientErrorException
{
    public BadRequestException() : base() { }
    public BadRequestException(string message) : base(message) { }
}