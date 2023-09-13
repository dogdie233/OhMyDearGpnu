namespace OhMyDearGpnu.Api.Responses
{
    public class Response
    {
        public readonly string? message;

        public Response(string? message)
        {
            this.message = message;
        }

        public bool IsSucceeded => message == null;
    }

    public class DataResponse<T> : Response
    {
        public readonly T? data;

        public DataResponse(string? message, T? value) : base(message)
        {
            this.data = value;
        }
    }
}
