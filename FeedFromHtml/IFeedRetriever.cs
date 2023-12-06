namespace FeedFromHtml;

public interface IFeedRetriever
{
    public byte[] Retrieve(FeedConfig feedConfig, string? appRoot);
}