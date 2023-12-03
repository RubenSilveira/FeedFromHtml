using System.Net;
using System.Text;

namespace FeedFromHtml;

public class FeedRequestHandler(ILogger<FeedRequestHandler> _logger, IFeedConfigProvider _feedConfigProvider, IFeedRetriever _feedRetriever)
{
    private readonly ILogger<FeedRequestHandler> logger = _logger;
    private readonly IFeedConfigProvider feedConfigProvider = _feedConfigProvider;
    private readonly IFeedRetriever feedRetriever = _feedRetriever;

    public async Task Handle(HttpContext context)
    {
        string? feedId = context.Request.RouteValues["feedId"] as string;

        logger.LogInformation($"Received {context.Request.Method} request for feed {feedId}");

        context.Response.Clear();

        if ("GET" != context.Request.Method && "HEAD" != context.Request.Method)
        {
            logger.LogWarning($"{context.Request.Method} isn't an accepted method");
            context.Response.StatusCode = (int) HttpStatusCode.MethodNotAllowed;
            return;
        }

        FeedConfig? feedConfig = feedConfigProvider.GetFeedConfig(feedId);
        if (null == feedConfig)
        {
            logger.LogWarning($"Feed {feedId} isn't recognized");
            context.Response.StatusCode = (int) HttpStatusCode.NotFound;
            return;
        }

        try
        {
            feedRetriever.Retrieve(feedConfig);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Feed {feedId} errored");
            context.Response.StatusCode = (int) HttpStatusCode.ServiceUnavailable;
            return;
        }

        context.Response.StatusCode = (int) HttpStatusCode.OK;
        context.Response.ContentType = "text/xml";
        context.Response.Headers.ContentEncoding = Encoding.UTF8.BodyName;
        context.Response.ContentLength = feedRetriever.ByteCount;

        await context.Response.StartAsync();
        await context.Response.BodyWriter.FlushAsync();

        if ("GET" == context.Request.Method)
        {
            feedRetriever.Write(context.Response.BodyWriter.AsStream());
        }

        await context.Response.CompleteAsync();

        logger.LogInformation($"Returned feed {feedId} to client - {feedRetriever.ByteCount} bytes");
    }
}