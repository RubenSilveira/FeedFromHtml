namespace FeedFromHtml;

public interface IFeedRetriever
{
    public void Retrieve(FeedConfig feedConfig);

    public void Write(Stream stream);

    public long ByteCount { get; }
}