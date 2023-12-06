namespace FeedFromHtml;

public interface IFeedConfigProvider
{
    public FeedConfig? GetFeedConfig(string feedId);

    public FeedConfig[] EnabledFeeds { get; }
}
