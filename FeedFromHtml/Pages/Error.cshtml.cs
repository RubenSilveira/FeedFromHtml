using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FeedFromHtml.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public string RequestPath { get; set; } = string.Empty;
    public DateTime RequestTimestamp { get; set; } = DateTime.MinValue.ToUniversalTime();
    public Exception? Error { get; set; }

    public void OnGet()
    {
        IExceptionHandlerPathFeature? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        Error = exceptionHandlerPathFeature?.Error;

        RequestPath = (HttpContext.Features.Get<HttpRequestExtensionFeature>()?.AppRoot[..^1] ?? string.Empty)
            + (exceptionHandlerPathFeature?.Path ?? HttpContext.Request.Path);

        RequestTimestamp = HttpContext.Features.Get<HttpRequestExtensionFeature>()?.TimestampUtc ?? DateTime.UtcNow;
    }
}