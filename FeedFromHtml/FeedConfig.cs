namespace FeedFromHtml;

public record FeedConfig
{
    public string FeedId { get; set; } = string.Empty;
    public bool Enabled { get; set; } = false;
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public int Ttl { get; set; } = 60;
    public string ImageUrl { get; set; } = string.Empty;
    public string XPathArticlesContainer { get; set; } = string.Empty;
    public string XPathTitleContainer { get; set; } = string.Empty;
    public string XPathHrefContainer { get; set; } = string.Empty;
    public string[] XPathDescriptionComponents { get; set; } = [];
}