namespace FeedFromHtml;

public class HttpRequestExtensionFeature(HttpContext _httpContext)
{
    private readonly HttpContext httpContext = _httpContext;

    public DateTime TimestampUtc { get; } = DateTime.UtcNow;

    public string AppRoot
    {
        get
        {
            string appRootFolder = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.PathBase}".ToLower();
            if (false == appRootFolder.EndsWith('/'))
            {
                appRootFolder += '/';
            }
            return appRootFolder;
        }
    }
}