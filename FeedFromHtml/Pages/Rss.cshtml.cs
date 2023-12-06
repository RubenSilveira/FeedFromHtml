using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FeedFromHtml.Pages;

public class RssModel(ILogger<RssModel> _logger, IFeedConfigProvider _feedConfigProvider, IFeedRetriever _feedRetriever) : PageModel
{
    private readonly ILogger<RssModel> logger = _logger;
    private readonly IFeedConfigProvider feedConfigProvider = _feedConfigProvider;
    private readonly IFeedRetriever feedRetriever = _feedRetriever;

    public IActionResult OnGet(string feedId)
    {
        try
        {
            logger.LogInformation($"Received {Request.Method} request for feed {feedId}");

            Response.Clear();

            FeedConfig? feedConfig = feedConfigProvider.GetFeedConfig(feedId);
            if (null == feedConfig)
            {
                logger.LogWarning($"Feed {feedId} isn't recognized");
                return NotFound();
            }

            byte[] result;
            try
            {
                result = feedRetriever.Retrieve(feedConfig, HttpContext.Features.Get<HttpRequestExtensionFeature>()?.AppRoot);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Feed {feedId} retrieval failed");
                return StatusCode((int) HttpStatusCode.ServiceUnavailable);
            }

            logger.LogInformation($"Returning feed {feedId} to client - {result.Length} bytes");

            return this.File(result, "text/xml; charset=utf-8");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Feed {feedId} processing failed");
            return StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
}