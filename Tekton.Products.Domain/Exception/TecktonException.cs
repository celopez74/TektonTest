namespace Tekton.Products.Domain.Exception;

public class TecktonException : System.Exception
{
    public int Code;
    public TecktonException() : base() { }
    public TecktonException(string message) : base(message) { }

    public TecktonException(int code, string message) : base(message)
    {
        Code = code;
    }
}
