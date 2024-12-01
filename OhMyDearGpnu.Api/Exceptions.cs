namespace OhMyDearGpnu.Api;

public class UnexpectedResponseException(string message) : Exception(message)
{
    public static async ValueTask<UnexpectedResponseException> FromHttpContentAsync(HttpResponseMessage response, string defaultMessage = "Invalid response data")
    {
        var message = await response.Content.ReadAsStringAsync();
        return new UnexpectedResponseException(string.IsNullOrEmpty(message) ? defaultMessage : message);
    }
}

public class WebAuthRequiredException : Exception
{
}