namespace OhMyDearGpnu.Api.Common.Drawing;

public class InvalidImageDataException : Exception
{
    public InvalidImageDataException(string message) : base(message)
    {
    }

    public InvalidImageDataException(string message, Exception innerException) : base(message, innerException)
    {
    }
}