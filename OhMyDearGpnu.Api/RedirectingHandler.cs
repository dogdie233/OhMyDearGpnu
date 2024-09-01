using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace OhMyDearGpnu.Api;

/// <summary>
/// A delegating handler that handles HTTP redirects (301, 302, 303, 307, and 308).
/// Copy from https://gist.github.com/joelverhagen/3be85bc0d5733756befa
/// </summary>
public class RedirectingHandler : DelegatingHandler
{
    /// <summary>
    /// The property key used to access the list of responses.
    /// </summary>
    public const string HISTORY_PROPERTY_KEY = "Knapcode.Http.Handlers.RedirectingHandler.ResponseHistory";

    private static readonly HashSet<HttpStatusCode> redirectStatusCodes =
    [
        HttpStatusCode.MovedPermanently,
        HttpStatusCode.Found,
        HttpStatusCode.SeeOther,
        HttpStatusCode.TemporaryRedirect,
        (HttpStatusCode)308
    ];

    private static readonly HashSet<HttpStatusCode> keepRequestBodyRedirectStatusCodes =
    [
        HttpStatusCode.TemporaryRedirect,
        (HttpStatusCode)308
    ];


    /// <summary>
    /// Initializes a new instance of the <see cref="RedirectingHandler"/> class.
    /// </summary>
    public RedirectingHandler()
    {
        AllowAutoRedirect = true;
        MaxAutomaticRedirections = 50;
        DownloadContentOnRedirect = true;
        KeepResponseHistory = true;
    }

    /// <summary>
    /// Gets or sets a value that indicates whether the handler should follow redirection responses.
    /// </summary>
    public bool AllowAutoRedirect { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of redirects that the handler follows.
    /// </summary>
    public int MaxAutomaticRedirections { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the response body should be downloaded before each redirection.
    /// </summary>
    public bool DownloadContentOnRedirect { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the response history should be saved to the <see cref="HttpResponseMessage.RequestMessage"/> properties with the key of <see cref="HISTORY_PROPERTY_KEY"/>.
    /// </summary>
    public bool KeepResponseHistory { get; set; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // buffer the request body, to allow re-use in redirects
        HttpContent? requestBody = null;
        if (AllowAutoRedirect && request.Content != null)
        {
            var buffer = await request.Content.ReadAsByteArrayAsync(cancellationToken);
            requestBody = new ByteArrayContent(buffer);
            foreach (var header in request.Content.Headers)
                requestBody.Headers.Add(header.Key, header.Value);
        }

        // make a copy of the request headers
        var requestHeaders = request
            .Headers
            .Select(p => new KeyValuePair<string, string[]>(p.Key, p.Value.ToArray()))
            .ToArray();

        // send the initial request
        var response = await base.SendAsync(request, cancellationToken);
        var responses = new List<HttpResponseMessage>();

        var redirectCount = 0;
        while (AllowAutoRedirect && redirectCount < MaxAutomaticRedirections && TryGetRedirectLocation(response, out var locationString))
        {
            if (DownloadContentOnRedirect)
                await response.Content.ReadAsByteArrayAsync(cancellationToken);

            var previousRequestUri = response.RequestMessage!.RequestUri!;

            // Credit where credit is due: https://github.com/kennethreitz/requests/blob/master/requests/sessions.py
            // allow redirection without a scheme
            if (locationString.StartsWith("//"))
                locationString = previousRequestUri.Scheme + ":" + locationString;

            var nextRequestUri = new Uri(locationString, UriKind.RelativeOrAbsolute);

            // allow relative redirects
            if (!nextRequestUri.IsAbsoluteUri)
                nextRequestUri = new Uri(previousRequestUri, nextRequestUri);

            // override previous method
            var nextMethod = response.RequestMessage.Method;
            if ((response.StatusCode == HttpStatusCode.Moved && nextMethod == HttpMethod.Post) ||
                (response.StatusCode == HttpStatusCode.Found && nextMethod != HttpMethod.Head) ||
                (response.StatusCode == HttpStatusCode.SeeOther && nextMethod != HttpMethod.Head))
            {
                nextMethod = HttpMethod.Get;
                requestBody = null;
            }

            if (!keepRequestBodyRedirectStatusCodes.Contains(response.StatusCode))
            {
                requestBody = null;
            }

            // build the next request
            var nextRequest = new HttpRequestMessage(nextMethod, nextRequestUri)
            {
                Content = requestBody,
                Version = request!.Version
            };

            foreach (var header in requestHeaders)
            {
                nextRequest.Headers.Add(header.Key, header.Value);
            }

            foreach (var pair in request.Options)
                nextRequest.Options.TryAdd(pair.Key, pair.Value);

            // keep a history all responses
            if (KeepResponseHistory)
                responses.Add(response);

            // send the next request
            response = await base.SendAsync(nextRequest, cancellationToken);

            request = response.RequestMessage!;
            redirectCount++;
        }

        // save the history to the request message properties
        if (KeepResponseHistory && response.RequestMessage != null)
        {
            responses.Add(response);
            response.RequestMessage.Options.TryAdd(HISTORY_PROPERTY_KEY, responses);
        }

        return response;
    }

    private static bool TryGetRedirectLocation(HttpResponseMessage response, [NotNullWhen(true)] out string? location)
    {
        if (redirectStatusCodes.Contains(response.StatusCode) &&
            response.Headers.TryGetValues("Location", out var locations) &&
            (locations = locations.ToArray()).Count() == 1 &&
            !string.IsNullOrWhiteSpace(locations.First()))
        {
            location = locations.First().Trim();
            return true;
        }

        location = null;
        return false;
    }
}