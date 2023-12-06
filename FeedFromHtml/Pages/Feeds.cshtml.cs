using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FeedFromHtml.Pages;

public class FeedsModel(IFeedConfigProvider _feedConfigProvider) : PageModel
{
    public readonly IFeedConfigProvider feedConfigProvider = _feedConfigProvider;
}