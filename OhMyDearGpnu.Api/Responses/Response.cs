using System.Diagnostics.CodeAnalysis;

namespace OhMyDearGpnu.Api.Responses
{
    public class Response
    {
        public readonly string? message;

        protected Response(string? message)
        {
            this.message = message;
        }

        public bool IsSucceeded => message == null;
        
        public static Response Success() => new(null);
        public static Response Fail(string message) => new(message);
        public static Response Fail(Exception exception) => new(exception.Message);
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
        
        public new static DataResponse<T> Success(T value) => new(null, value);
        public new static DataResponse<T> Fail(string message) => new(message, default);
        public new static DataResponse<T> Fail(Exception exception) => new(exception.Message, default);
    }
}
