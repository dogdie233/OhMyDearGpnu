using System.Diagnostics.CodeAnalysis;

namespace OhMyDearGpnu.Api.Common;

public class Response
{
    public readonly string? message;

    protected Response(string? message)
    {
        this.message = message;
    }

    public bool IsSucceeded => message == null;

    public static Response Success()
    {
        return new Response(null);
    }

    public static Response Fail(string message)
    {
        return new Response(message);
    }

    public static Response Fail(Exception exception)
    {
        return new Response(exception.Message);
    }
}

public class DataResponse<T> : Response
{
    public readonly T? data;

    protected DataResponse(string? message, T? data) : base(message)
    {
        this.data = data;
    }

    [MemberNotNullWhen(true, nameof(data))]
    public new bool IsSucceeded => message == null;

    public new static DataResponse<T> Success(T value)
    {
        return new DataResponse<T>(null, value);
    }

    public new static DataResponse<T> Fail(string message)
    {
        return new DataResponse<T>(message, default);
    }

    public new static DataResponse<T> Fail(Exception exception)
    {
        return new DataResponse<T>(exception.Message, default);
    }
}